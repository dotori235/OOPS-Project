using System.Collections.Generic;
using UnityEngine;

namespace Backend
{
    public abstract class Item : MonoBehaviour, IUpgradable, ISellable
    {
        // 가격 계수: 스탯 추가 시 여기에 계수만 등록하면 된다 (OCP)
        private static readonly Dictionary<StatType, float> PriceCoefficients = new Dictionary<StatType, float>
        {
            { StatType.AttackPower, 2.0f },
            { StatType.Durability,  1.0f },
            { StatType.Splendor,    1.0f },
        };
        
        private Stat _stats = new Stat();
        private bool _isDefective;
        private Vector3 _position;

        public bool IsDefective { get => _isDefective; protected set => _isDefective = value; }
        public Vector3 Position { get => _position; set => _position = value; }

        public void Initialize(Stat stats)
        {
            _stats = stats ?? new Stat();
            _isDefective = false;
            
        }

        public void MoveItem(Vector3 dp)
        {
            _position += dp;
            transform.position = _position;
        }

        public virtual void Upgrade(StatType stat, float amount)
        {
            _stats.Add(stat, amount);
        }

        public virtual Stat GetStats()
        {
            return _stats.Clone();
        }

        public virtual float CalculatePrice(float spMult)
        {
            float basePrice = 0f;
            foreach (var coefficient in PriceCoefficients)
            {
                basePrice += _stats.Get(coefficient.Key) * coefficient.Value;
            }
            basePrice += _stats.Get(StatType.Splendor) * spMult;
            return basePrice;
        }

        public virtual float CalculateDefectChance()
        {
            // Simple defect chance formula based on durability
            float chance = 0.05f * (5 / (_stats.Get(StatType.Durability) + 5));
            return Mathf.Clamp(chance, 0.01f, 0.9f);
        }

        public void MakeDefective()
        {
            _isDefective = true;
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
        }

        public void SellItem()
        {
            Destroy(gameObject);
        }
    }
}
