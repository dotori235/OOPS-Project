using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework.Constraints;
namespace Backend
{
    public class BeltTrack : MonoBehaviour, IBeltTrackLevelSubject
    {
        [SerializeField] private SellManager _sellManager;
        [SerializeField] private float _trackLength = 3.5f;

        private int _level = 1;
        private int _machineSpaces = 3;
        private Vector3 _speed = new Vector3(1, 0, 0);

        private readonly List<Item> _items = new List<Item>();
        private readonly Dictionary<Item, float> _itemPositions = new Dictionary<Item, float>();
        private List<IObserver> _observers = new List<IObserver>();
        public int Level { get => _level; private set => _level = value; }
        public int MachineSpaces { get => _machineSpaces; private set => _machineSpaces = value; }
        public Vector3 Speed { get => _speed; private set => _speed = value; }
        public float TrackLength { get => _trackLength; private set => _trackLength = value; }

        private void Update()
        {
            List<Item> reachedEnd = new List<Item>();

            for (int i = 0; i < _items.Count; i++)
            {
                Item item = _items[i];
                float currentPos = _itemPositions[item];
                //float nextPos = currentPos + _speed * Time.deltaTime;
                item.MoveItem(_speed * Time.deltaTime);
                /*
                if (nextPos >= _trackLength)
                {
                    reachedEnd.Add(item);
                }
                else
                {
                    _itemPositions[item] = nextPos;
                }*/
                if(item.Position.x >= _trackLength)
                {
                    reachedEnd.Add(item);
                }
            }

            foreach (var item in reachedEnd)
            {
                RemoveItem(item);
                if (_sellManager != null)
                {
                    _sellManager.SellItem(item);
                }
            }
            if(Input.GetKeyDown(KeyCode.P)) LevelUp();
        }

        public void AddItem(Item item)
        {
            if (item == null) return;
            if (!_items.Contains(item))
            {
                _items.Add(item);
                _itemPositions[item] = 0f;
            }
        }

        public void RemoveItem(Item item)
        {
            if (item == null) return;
            if (_items.Contains(item))
            {
                _items.Remove(item);
                _itemPositions.Remove(item);
            }
        }

        public Item GetNearestItem(float xPosition)
        {
            Item nearest = null;
            float minDistance = float.MaxValue;

            foreach (var item in _items)
            {
                float itemPos = item.transform.position.x;
                float dist = Mathf.Abs(itemPos - xPosition);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    nearest = item;
                }
            }

            if (minDistance <= 1.5f)
            {
                return nearest;
            }
            return null;
        }

        public void LevelUp()
        {
            _level++;
            _machineSpaces += 1;
            _trackLength += 1;
            NotifyBeltTrackLevel();
        }

        public int GetMachineSpaces()
        {
            return _machineSpaces;
        }

        public void RegisterObserver(IObserver observer)
        {
            Debug.Log("regi");
            if (_observers.Contains(observer)) return;
            _observers.Add(observer);
        }
        public void UnregisterObserver(IObserver observer)
        {
            if (!_observers.Contains(observer)) return;
            _observers.Remove(observer);
        }
        public void NotifyObservers()
        {

        }
        public void NotifyBeltTrackLevel()
        {
            foreach(IObserver ob in _observers)
            {
                if(ob is IBeltTrackLevelObserver tlOb)
                {
                    tlOb.OnBeltTrackLevelChanged();
                }
            }
        }
    }
}
