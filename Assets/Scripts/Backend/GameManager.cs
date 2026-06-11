using UnityEngine;

namespace Backend
{
    public class GameManager : MonoBehaviour, IGameEventListener
    {
        private readonly GameStateMachine _stateMachine = new GameStateMachine();

        private readonly ReadyState _readyState = new ReadyState();
        private readonly PlayingState _playingState = new PlayingState();
        private readonly PausedState _pausedState = new PausedState();
        private readonly GameOverState _gameOverState = new GameOverState();

        private void Awake()
        {
            _stateMachine.ChangeState(_readyState);
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
            _stateMachine.Update();
        }

        public void OnEvent(GameEvent e)
        {
            if (e is BankruptcyEvent)
            {
                _stateMachine.ChangeState(_gameOverState);
            }
        }

        public void StartGame()
        {
            if (_stateMachine.Current is ReadyState)
            {
                _stateMachine.ChangeState(_playingState);
            }
        }

        public void PauseGame()
        {
            if (_stateMachine.Current is PlayingState)
            {
                _stateMachine.ChangeState(_pausedState);
            }
        }

        public void ResumeGame()
        {
            if (_stateMachine.Current is PausedState)
            {
                _stateMachine.ChangeState(_playingState);
            }
        }

        public void ResetGame()
        {
            FactoryStatus.GetInstance().ResetStatus();
            _stateMachine.ChangeState(_playingState);
        }
    }
}
