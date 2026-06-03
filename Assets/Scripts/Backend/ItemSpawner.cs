using UnityEngine;

namespace Backend
{
    public class ItemSpawner : MonoBehaviour
    {
        [SerializeField] private BeltTrack _beltTrack;
        [SerializeField] private GameObject _itemPrefab;

        [SerializeField] private float _baseAP = 10f;
        [SerializeField] private float _baseDU = 100f;
        [SerializeField] private float _baseSP = 0f;
        [SerializeField] private float _spawnInterval = 3f;

        private float _timer = 0f;
        private int _level = 1;

        public float BaseAP { get => _baseAP; private set => _baseAP = value; }
        public float BaseDU { get => _baseDU; private set => _baseDU = value; }
        public float BaseSP { get => _baseSP; private set => _baseSP = value; }
        public float SpawnInterval { get => _spawnInterval; private set => _spawnInterval = value; }

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

            Item newItem = new StandardItem(_baseAP, _baseDU, _baseSP);
            _beltTrack.AddItem(newItem);

            if (_itemPrefab != null)
            {
                Instantiate(_itemPrefab, transform.position, Quaternion.identity);
            }
        }

        public void LevelUp()
        {
            _level++;
            _baseAP += 2f;
            _baseDU += 10f;
            _spawnInterval = Mathf.Max(0.5f, _spawnInterval * 0.9f);
        }
    }
}
