using UnityEngine;

namespace Backend
{
    public abstract class GameEvent
    {
        public float Timestamp { get; private set; }

        protected GameEvent()
        {
            Timestamp = Time.time;
        }
    }
}
