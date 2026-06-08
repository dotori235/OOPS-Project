using System.Collections.Generic;
using UnityEngine;

namespace Backend
{
    public class GameManager : MonoBehaviour, IGameEventListener
    {
        [SerializeField] private BeltTrack _beltTrack;
        [SerializeField] private ItemSpawner _itemSpawner;

        [SerializeField] private GameObject _grinderPrefab;
        [SerializeField] private GameObject _welderPrefab;
        [SerializeField] private GameObject _painterPrefab;

        private readonly List<IManager> _managers = new List<IManager>();
        private readonly List<Machine> _placedMachines = new List<Machine>();

        private void Awake()
        {

            // Find round manager in scene
            var roundManager = FindAnyObjectByType<RoundManager>();
            if (roundManager != null) _managers.Add(roundManager);

            // Find sell manager in scene
            var sellManager = FindAnyObjectByType<SellManager>();
            if (sellManager != null) _managers.Add(sellManager);
        }

        private void Start()
        {
            EventBus.GetInstance().Subscribe(this);
            StartGame();
        }

        private void OnDestroy()
        {
            EventBus.GetInstance().Unsubscribe(this);
        }

        private void Update()
        {
            if (FactoryStatus.GetInstance().IsGameOver())
            {
                PauseGame();
                Debug.Log("[GameManager] Game Over! Bankruptcy threshold reached.");
            }
        }

        public void OnEvent(GameEvent e)
        {
            Debug.Log($"[GameManager] Coordinated Event: {e.GetType().Name} at {e.Timestamp}");
        }

        public void StartGame()
        {
            Time.timeScale = 1f;
            Debug.Log("[GameManager] Game started!");
        }

        public void PauseGame()
        {
            Time.timeScale = 0f;
            Debug.Log("[GameManager] Game paused.");
        }

        public void ResetGame()
        {
            FactoryStatus.GetInstance().ResetStatus();

            foreach (var machine in _placedMachines)
            {
                if (machine != null)
                {
                    Destroy(machine.gameObject);
                }
            }
            _placedMachines.Clear();

            int spaces = _beltTrack != null ? _beltTrack.MachineSpaces : 3;
            for (int i = 0; i < spaces; i++)
            {
                _placedMachines.Add(null);
            }

            Time.timeScale = 1f;
            Debug.Log("[GameManager] Game reset successfully.");
        }

        public void PlaceMachine(MachineType type, int pos)
        {
            if (_beltTrack == null) return;

            int targetSpaces = _beltTrack.MachineSpaces;
            while (_placedMachines.Count < targetSpaces)
            {
                _placedMachines.Add(null);
            }

            if (pos < 0 || pos >= targetSpaces)
            {
                Debug.LogWarning($"[GameManager] Cannot place machine at slot {pos}. Invalid index.");
                return;
            }

            if (_placedMachines[pos] != null)
            {
                Debug.LogWarning($"[GameManager] Slot {pos} is already occupied.");
                return;
            }

            GameObject prefab = null;
            switch (type)
            {
                case MachineType.Grinder: prefab = _grinderPrefab; break;
                case MachineType.Welder:  prefab = _welderPrefab; break;
                case MachineType.Painter: prefab = _painterPrefab; break;
            }

            if (prefab == null)
            {
                GameObject obj = new GameObject(type.ToString());
                obj.transform.position = new Vector3(pos * 2.0f, 0f, 0f);
                Machine machine = null;
                switch (type)
                {
                    case MachineType.Grinder: machine = obj.AddComponent<Grinder>(); break;
                    case MachineType.Welder:  machine = obj.AddComponent<Welder>(); break;
                    case MachineType.Painter: machine = obj.AddComponent<Painter>(); break;
                }
                if (machine != null)
                {
                    machine.Configure(_beltTrack);
                    _placedMachines[pos] = machine;
                }
            }
            else
            {
                GameObject obj = Instantiate(prefab, new Vector3(pos * 2.0f, 0f, 0f), Quaternion.identity);
                Machine machine = obj.GetComponent<Machine>();
                if (machine != null)
                {
                    machine.Configure(_beltTrack);
                    _placedMachines[pos] = machine;
                }
            }
        }

        public void LevelUpMachine(int pos)
        {
            if (pos >= 0 && pos < _placedMachines.Count && _placedMachines[pos] != null)
            {
                _placedMachines[pos].LevelUp();
                Debug.Log($"[GameManager] Leveled up machine at slot {pos} to Level {_placedMachines[pos].Level}.");
            }
        }

        public void RemoveMachine(int pos)
        {
            if (pos >= 0 && pos < _placedMachines.Count && _placedMachines[pos] != null)
            {
                Destroy(_placedMachines[pos].gameObject);
                _placedMachines[pos] = null;
                Debug.Log($"[GameManager] Removed machine from slot {pos}.");
            }
        }

        public void LevelUpBelt()
        {
            if (_beltTrack != null)
            {
                _beltTrack.LevelUp();
                Debug.Log($"[GameManager] Upgraded Belt Track! New speed: {_beltTrack.Speed}, New machine slots: {_beltTrack.MachineSpaces}");
            }
        }
    }
}
