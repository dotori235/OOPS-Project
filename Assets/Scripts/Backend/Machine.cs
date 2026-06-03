using UnityEngine;

namespace Backend
{
    public abstract class Machine : MonoBehaviour
    {
        [SerializeField] protected BeltTrack _beltTrack;
        [SerializeField] protected float _upgradeInterval = 1f;

        protected int _level = 1;
        protected float _cooldown = 0f;
        protected float _upgradeAmount = 1f;

        public int Level { get => _level; protected set => _level = value; }
        public float UpgradeInterval { get => _upgradeInterval; protected set => _upgradeInterval = value; }
        public float UpgradeAmount { get => _upgradeAmount; protected set => _upgradeAmount = value; }

        protected virtual void Update()
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

        protected virtual void UpgradeItem(IUpgradable item)
        {
            StatType stat = GetTargetStat();
            item.Upgrade(stat, _upgradeAmount);

            if (CanCauseDefect())
            {
                // Machine level increases precision and reduces defect chance
                float defectChance = 0.05f / _level;
                if (Random.value < defectChance && item is Item concreteItem)
                {
                    concreteItem.MakeDefective();
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
    }
}
