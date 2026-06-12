using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
namespace Backend
{
    public class GameManager : MonoBehaviour, IGameEventListener, IGameStateSubject
    {
        private List<IObserver> _observers = new List<IObserver>();
        private readonly GameStateMachine _stateMachine = new GameStateMachine();

        private readonly PlayingState _playingState = new PlayingState();
        private readonly PausedState _pausedState = new PausedState();
        private readonly GameOverState _gameOverState = new GameOverState();

        private void Awake()
        {
            SetGameState(_playingState);
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
                SetGameState(_gameOverState);
            }
        }
        public void PauseResume()
        {
            if (_stateMachine.Current is PlayingState) PauseGame();
            else if (_stateMachine.Current is PausedState) ResumeGame();
        }
        public void StartGame()
        {

            SetGameState(_playingState);
            
        }

        public void PauseGame()
        {
            if (_stateMachine.Current is PlayingState)
            {
                SetGameState(_pausedState);
            }
        }

        public void ResumeGame()
        {
            if (_stateMachine.Current is PausedState)
            {
                SetGameState(_playingState);
            }
        }

        private void SetGameState(IGameState state)
        {
            _stateMachine.ChangeState(state);
            NotifyGameState();
        }
        public void ResetGame()
        {
            SceneManager.LoadScene("FrontEnd");
            //FactoryStatus.GetInstance().ResetStatus();
            //_stateMachine.ChangeState(_playingState);
        }
        public void SetTimeScale(float timeScale)
        {
            _playingState.TimeScale = timeScale;
        }
        public void RegisterObserver(IObserver observer)
        {
            if (_observers.Contains(observer)) return;
            _observers.Add(observer);
        }
        public void UnregisterObserver(IObserver observer)
        {
            if(!_observers.Contains(observer)) return;
            _observers.Remove(observer);
        }
        public void NotifyObservers()
        {
            
        }
        public void NotifyGameState()
        {
            foreach (var observer in _observers)
            {
                if (observer is IGameStateObserver gsOb)
                {
                    gsOb.OnGameStateChanged(_stateMachine.Current);
                }
            }
        }
    }
}
