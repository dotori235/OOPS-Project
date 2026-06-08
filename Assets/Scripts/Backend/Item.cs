using System.Collections.Generic;
using UnityEngine;

namespace Backend
{
    public abstract class Item : MonoBehaviour,IUpgradable, ISellable
    {
        protected float _attackPower;
        protected float _durability;
        protected float _splendor;
        protected bool _isDefective;
        protected Vector3 _position;

        public float AttackPower { get => _attackPower; protected set => _attackPower = value; }
        public float Durability { get => _durability; protected set => _durability = value; }
        public float Splendor { get => _splendor; protected set => _splendor = value; }
        public bool IsDefective { get => _isDefective; protected set => _isDefective = value; }
        public Vector3 Position { get => _position; set => _position = value; }
        public void SetValue(float attackPower, float durability, float splendor)
        {
            _attackPower = attackPower;
            _durability = durability;
            _splendor = splendor;
            _isDefective = false;
        }
        
        public void MoveItem(Vector3 dp)
        {
            _position += dp;
            transform.position = _position;
        }
        public virtual void Upgrade(StatType stat, float amount)
        {
            
            switch (stat)
            {
                case StatType.AttackPower:
                    _attackPower += amount;
                    break;
                case StatType.Durability:
                    _durability += amount;
                    break;
                case StatType.Splendor:
                    _splendor += amount;
                    break;
            }
        }

        public virtual Dictionary<StatType, float> GetStats()
        {
            return new Dictionary<StatType, float>
            {
                { StatType.AttackPower, _attackPower },
                { StatType.Durability, _durability },
                { StatType.Splendor, _splendor }
            };
        }

        public virtual float CalculatePrice(float spMult)
        {
            
            float basePrice = _attackPower * 2.0f + _durability * 1.0f;
            float splendorBonus = 1.0f + (_splendor * spMult * 0.05f);
            return basePrice * splendorBonus;
        }

        public virtual float CalculateDefectChance()
        {
            // Simple defect chance formula based on durability
            float chance = 0.3f * (5/(_durability + 5));
            return UnityEngine.Mathf.Clamp(chance, 0.01f, 0.9f);
        }

        public void MakeDefective()
        {
            _isDefective = true;
            transform.GetComponent<Renderer>().material.color = Color.red;
        }
    }
}
