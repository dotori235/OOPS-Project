using UnityEngine;

namespace Backend
{
    public class GameOverState : IGameState
    {
        public void Enter()
        {
            Time.timeScale = 0f;
            Debug.Log("[GameStateMachine] Game Over! Bankruptcy threshold reached.");
        }

        public void Update() { }
        public void Exit() { }
    }
}
