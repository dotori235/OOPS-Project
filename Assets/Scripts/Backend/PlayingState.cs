using UnityEngine;

namespace Backend
{
    public class PlayingState : IGameState
    {
        private float _timeScale = 1;
        public float TimeScale { get=>_timeScale; set => _timeScale = value; }
        public void Enter()
        {
            
            Debug.Log("[GameStateMachine] Playing.");
            DebugLog.Instance.Print("[GameStateMachine] Playing.");
        }
        
        public void Update() { Time.timeScale = _timeScale; }
        public void Exit() { }
    }
}
