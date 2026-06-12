using System;
using UnityEngine;

namespace Backend
{
    [Serializable]
    public struct StatEntry
    {
        [SerializeField] private StatType _type;
        [SerializeField] private float _value;

        public StatType Type => _type;
        public float Value => _value;

        public StatEntry(StatType type, float value)
        {
            _type = type;
            _value = value;
        }
    }
}
