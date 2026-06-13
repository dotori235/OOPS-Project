using System.Collections.Generic;
using UnityEngine;

namespace Backend
{
    public class BeltTrack : MonoBehaviour, IBeltTrackLevelSubject, ISellBlockObserver
    {
        [SerializeField] private SellManager _sellManager;
        // 타일 수 기반 트랙: 기계 타일 _tileCount개 + 끝의 판매 타일 1개
        [SerializeField] private int _tileCount = 3;
        [SerializeField] private float _tileSize = 1f;
        [SerializeField] private BlockBase _sellBlock;

        private int _level = 1;
        private Vector3 _speed = new Vector3(1, 0, 0);

        private readonly List<Item> _items = new List<Item>();
        private List<IObserver> _observers = new List<IObserver>();

        public int Level { get => _level; private set => _level = value; }
        public int TileCount { get => _tileCount; private set => _tileCount = value; }
        public float TileSize { get => _tileSize; private set => _tileSize = value; }
        public int MachineSpaces { get => _tileCount; }
        public Vector3 Speed { get => _speed; private set => _speed = value; }
        public float TrackLength { get => _tileCount * _tileSize; }
        public float LevelUpPrice { get => _level * 500; }

        private void Start()
        {
            _sellManager = SellManager.Instance;
            if(_sellBlock is ISellBlockSubject sb)
                sb.RegisterObserver(this);
        }

        private void Update()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                _items[i].MoveItem(_speed * Time.deltaTime);
            }
        }

        public void AddItem(Item item)
        {
            if (item == null) return;
            if (!_items.Contains(item))
            {
                _items.Add(item);
            }
        }

        public void RemoveItem(Item item)
        {
            if (item == null) return;
            _items.Remove(item);
        }

        public void LevelUp()
        {
            _level++;
            _tileCount += 1;
            _sellBlock.transform.localPosition = new Vector3(TrackLength + _tileSize, 0f, 0f);

            NotifyBeltTrackLevel();
        }

        public void RegisterObserver(IObserver observer)
        {
            if (_observers.Contains(observer)) return;
            _observers.Add(observer);
            if(observer is IBeltTrackLevelObserver bo && _level > 1)
            {
                bo.OnBeltTrackLevelChanged(this);
            }
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
                    tlOb.OnBeltTrackLevelChanged(this);
                }
            }
        }
        public void OnNotify(ISubject subject)
        {

        }
        public void OnSellBlockReached(ISellable item)
        {
            _sellManager.SellItem(item);
            RemoveItem(item as Item);
        }
    }
}
