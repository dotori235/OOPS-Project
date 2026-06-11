using UnityEngine;

namespace Backend
{
    public class ItemSpawner : MonoBehaviour, IBeltTrackLevelObserver
    {
        [SerializeField] private BeltTrack _beltTrack;
        [SerializeField] private GameObject _itemPrefab;

        [SerializeField] private Stat _baseStat = new Stat();
        [SerializeField] private float _spawnInterval = 3f;

        private float _timer = 0f;
        private int _level = 1;

        public Stat BaseStat { get => _baseStat; private set => _baseStat = value; }
        public float SpawnInterval { get => _spawnInterval; private set => _spawnInterval = value; }
        private void Start()
        {
            _beltTrack?.RegisterObserver(this);
        }
        private void OnDestroy()
        {
            _beltTrack?.UnregisterObserver(this);
        }
        private void Update()
        {
            _timer += Time.deltaTime;
            if (ShouldSpawn())
            {
                SpawnItem();
                _timer = 0f;
            }
        }

        public bool ShouldSpawn()
        {
            return _timer >= _spawnInterval;
        }

        public void SpawnItem()
        {
            if (_beltTrack == null) return;


            if (_itemPrefab != null)
            {
                GameObject go = Instantiate(_itemPrefab, transform.position, Quaternion.identity, transform);

                Item item = go.GetComponent<Item>();
                _beltTrack.AddItem(item);
                item.Position = transform.position;
                item.Initialize(_baseStat.Clone());
            }
        }

        public void LevelUp()
        {
            _level++;
            _baseStat.Add(StatType.AttackPower, 2f);
            _baseStat.Add(StatType.Durability, 2f);
            _spawnInterval = Mathf.Max(0.5f, _spawnInterval * 0.9f);
        }
        public void OnNotify(ISubject subject)
        {

        }
        public void OnBeltTrackLevelChanged(IBeltTrackLevelSubject subject)
        {
            LevelUp();
        }
    }

}
