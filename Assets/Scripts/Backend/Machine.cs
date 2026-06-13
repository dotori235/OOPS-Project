using System.Collections;
using UnityEngine;
using System.Collections.Generic;
namespace Backend
{
    public abstract class Machine : MonoBehaviour, IMachineSubject
    {
        [SerializeField] private float _upgradeInterval = 1f;
        [SerializeField] private float _maxHp = 100f;
        [SerializeField] private float _hpLossRate = 0.01f;
        [SerializeField] private float _minUpgradableHp = 30f;
        [SerializeField] private int _wearFreeLevel = 5;
        private int _level = 1;
        private float _cooldown = 0f;
        private float _upgradeAmount = 1f;
        private float _hp;
        private Renderer   _renderer;
        private MachineType _type;
        private List<IObserver> _observers = new List<IObserver>();
        public int Level { get => _level; private set => _level = value; }
        public float UpgradeInterval { get => _upgradeInterval; private set => _upgradeInterval = value; }
        public float UpgradeAmount { get => _upgradeAmount; private set => _upgradeAmount = value; }
        public float Hp { get => _hp; private set => _hp = value; }
        public float MaxHp { get => _maxHp; private set => _maxHp = value; }
        public float HpRatio { get => _maxHp <= 0f ? 0f : _hp / _maxHp; }
        public float LevelUpPrice { get => Level * Machine.LevelUpPriceCoeff; }
        public static float InstallPrice { get => 200f; }
        public static float LevelUpPriceCoeff { get => 100; }
        public static float RepairPrice { get => 50f; }
        // HP must be ready before any caller reads it (UI can query the same
        // frame the machine is instantiated, before Start), so initialize in Awake.
        protected virtual void Awake()
        {
            _hp = _maxHp;
        }
        private void Start()
        {
            _renderer = GetComponent<Renderer>();
            SetAlpha(0.2f);
        }
        private void Update()
        {
            if (_cooldown > 0f)
            {
                _cooldown -= Time.deltaTime;
            }
        }
        public MachineType GetMachineType()
        {
            return _type;
        }
        protected virtual void SetMachineType(MachineType type)
        {
            _type = type;
        }
        private void UpgradeItem(Item item)
        {
            ApplyWear();
            StartCoroutine(alphaEff());
            StatType stat = GetTargetStat();
            item.Upgrade(stat, _upgradeAmount);

            if (CanCauseDefect())
            {

                // Machine level increases precision and reduces defect chance
                float defectChance = item.CalculateDefectChance();
                if (Random.Range(0,1f) < defectChance)
                {
                    item.MakeDefective();
                }
            }
        }

        // Each processed item wears HP down, less so at higher levels;
        // no wear at/after _wearFreeLevel, and HP never drops below 0.
        private void ApplyWear()
        {
            int wearSteps = Mathf.Max(0, _wearFreeLevel - _level);
            if (wearSteps == 0) return;
            _hp = Mathf.Max(0f, _hp - _maxHp * _hpLossRate * wearSteps);
            NotifyMachine();
        }

        public bool CanLevelUp()
        {
            return _hp >= _minUpgradableHp;
        }

        public bool CanRepair()
        {
            return _hp < _maxHp;
        }

        public virtual void LevelUp()
        {
            if (!CanLevelUp()) return;

            _level++;
            _upgradeAmount += 0.5f;
            _upgradeInterval = Mathf.Max(0.2f, _upgradeInterval * 0.8f);
            NotifyMachine();
        }

        public void Repair()
        {
            if (!CanRepair()) return;
            _hp = _maxHp;
            NotifyMachine();

        }

        public abstract StatType GetTargetStat();
        public abstract bool CanCauseDefect();

        private void SetAlpha(float alpha)
        {
            Color color = _renderer.material.color;
            color.a = alpha;
            _renderer.material.color = color;
        }
        private IEnumerator alphaEff()
        {
            SetAlpha(1.0f);
            yield return new WaitForSeconds(0.05f);
            SetAlpha(0.2f);
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.tag == "Item" && _cooldown<=0 && _hp>0)
            {
                UpgradeItem(other.GetComponent<Item>());
                _cooldown = _upgradeInterval;
            }
        }

        public void RegisterObserver(IObserver observer)
        {
            if (_observers.Contains(observer)) return;
            _observers.Add(observer);
            if(observer is IMachineObserver mo)
            {
                mo.OnMachineChanged(this);
            }
        }
        public void UnregisterObserver(IObserver observer)
        {
            if(!_observers.Contains(observer)) return;
            _observers.Remove(observer);
        }
        public void NotifyObservers()
        {

        }
        public void NotifyMachine()
        {
            foreach(var observer in _observers)
            {
                if(observer is IMachineObserver mOb)
                {
                    mOb.OnMachineChanged(this);
                }
            }
        }
    }
}
