using System.Collections.Generic;

namespace Backend
{
    public class EventBus
    {
        private static EventBus _instance;
        private readonly List<IGameEventListener> _listeners = new List<IGameEventListener>();

        private EventBus() { }

        public static EventBus GetInstance()
        {
            if (_instance == null)
            {
                _instance = new EventBus();
            }
            return _instance;
        }

        public void Subscribe(IGameEventListener listener)
        {
            if (!_listeners.Contains(listener))
            {
                _listeners.Add(listener);
            }
        }

        public void Unsubscribe(IGameEventListener listener)
        {
            if (_listeners.Contains(listener))
            {
                _listeners.Remove(listener);
            }
        }

        public void Publish(GameEvent e)
        {
            // Use a copy to prevent modification of _listeners while traversing during an event
            var targets = new List<IGameEventListener>(_listeners);
            foreach (var listener in targets)
            {
                listener?.OnEvent(e);
            }
        }
    }
}
