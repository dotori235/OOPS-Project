using System;
using System.Collections.Generic;
using UnityEngine;

namespace Backend
{
    [Serializable]
    public class Stat : ISerializationCallbackReceiver
    {
        // 인스펙터 직렬화용. 런타임에는 _values가 단일 진실 공급원이다.
        [SerializeField] private List<StatEntry> _entries = new List<StatEntry>();

        private Dictionary<StatType, float> _values = new Dictionary<StatType, float>();

        public float Get(StatType type)
        {
            return _values.TryGetValue(type, out float value) ? value : 0f;
        }

        public void Set(StatType type, float value)
        {
            _values[type] = value;
        }

        public void Add(StatType type, float amount)
        {
            _values[type] = Get(type) + amount;
        }

        public Stat Clone()
        {
            Stat clone = new Stat();
            foreach (var pair in _values)
            {
                clone._values[pair.Key] = pair.Value;
            }
            return clone;
        }

        public void OnBeforeSerialize()
        {
            if (_values.Count == 0) return;
            _entries.Clear();
            foreach (var pair in _values)
            {
                _entries.Add(new StatEntry(pair.Key, pair.Value));
            }
        }

        public void OnAfterDeserialize()
        {
            _values.Clear();
            foreach (StatEntry entry in _entries)
            {
                _values[entry.Type] = entry.Value;
            }
        }
    }
}
