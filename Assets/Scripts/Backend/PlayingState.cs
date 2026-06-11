using UnityEngine;

namespace Backend
{
    public class PlayingState : IGameState
    {
        public void Enter()
        {
            Time.timeScale = 1f;
            Debug.Log("[GameStateMachine] Playing.");
        }

        public void Update() { }
        public void Exit() { }
    }
}
