namespace Backend
{
    public class GameStateMachine
    {
        private IGameState _current;

        public IGameState Current => _current;

        public void ChangeState(IGameState next)
        {
            _current?.Exit();
            _current = next;
            _current?.Enter();
        }

        public void Update()
        {
            _current?.Update();
        }
    }
}
