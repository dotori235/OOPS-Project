using UnityEngine;

namespace Backend
{
    public class PausedState : IGameState
    {
        public void Enter()
        {
            Time.timeScale = 0f;
            Debug.Log("[GameStateMachine] Paused.");
            DebugLog.Instance.Print("[GameStateMachine] Paused.");
        }

        public void Update() { }
        public void Exit() { }
    }
}
