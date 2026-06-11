using System.Collections;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

namespace Backend
{
    public abstract class Machine : MonoBehaviour
    {
        [SerializeField] private BeltTrack _beltTrack;
        [SerializeField] private float _upgradeInterval = 1f;
        private int _level = 1;
        private float _cooldown = 0f;
        private float _upgradeAmount = 1f;
        private Renderer   _renderer;
        private MachineType _type;
        public int Level { get => _level; private set => _level = value; }
        public float UpgradeInterval { get => _upgradeInterval; private set => _upgradeInterval = value; }
        public float UpgradeAmount { get => _upgradeAmount; private set => _upgradeAmount = value; }
        
        public static float InstallPrice { get => 200f; }
        public static float LevelUpPriceCoeff { get => 100; }
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
                return;
            }

            if (_beltTrack == null) return;

            // Get nearest item based on the machine's X position as the tracking position
            Item target = _beltTrack.GetNearestItem(transform.position.x);

            if (target != null)
            {
                UpgradeItem(target);
                _cooldown = _upgradeInterval;
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

        public virtual void Configure(BeltTrack beltTrack)
        {
            _beltTrack = beltTrack;
        }

        public virtual void LevelUp()
        {
            _level++;
            _upgradeAmount += 0.5f;
            _upgradeInterval = Mathf.Max(0.2f, _upgradeInterval * 0.9f);
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
            if (other.tag == "Item" && _cooldown<=0)
            {
                UpgradeItem(other.GetComponent<Item>());
                _cooldown = _upgradeInterval;
            }
        }

    }
}
