using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace Backend
{
    public class ItemSpawner : MonoBehaviour, IBeltTrackLevelObserver
    {
        [SerializeField] private BeltTrack _beltTrack;
        [SerializeField] private GameObject _itemPrefab;

        [SerializeField] private float _baseAP = 10f;
        [SerializeField] private float _baseDU = 0;
        [SerializeField] private float _baseSP = 0f;
        [SerializeField] private float _spawnInterval = 3f;
        
        private float _timer = 0f;
        private int _level = 1;

        public float BaseAP { get => _baseAP; private set => _baseAP = value; }
        public float BaseDU { get => _baseDU; private set => _baseDU = value; }
        public float BaseSP { get => _baseSP; private set => _baseSP = value; }
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

                _beltTrack.AddItem(go.GetComponent < Item >());
                go.GetComponent<Item>().Position = transform.position;
                go.GetComponent<Item>().SetValue(_baseAP, _baseDU, _baseSP);
            }
        }

        public void LevelUp()
        {
            _level++;
            _baseAP += 2f;
            _baseDU += 2f;
            _spawnInterval = Mathf.Max(0.5f, _spawnInterval * 0.9f);
        }
        public void OnNotify(ISubject subject)
        {

        }
        public void OnBeltTrackLevelChanged()
        {
            Debug.Log("levelup is");
            LevelUp();
        }
    }

}
