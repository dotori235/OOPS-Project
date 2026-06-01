namespace Backend
{
    public class MarketEvent : GameEvent
    {
        public EventType EventType { get; private set; }

        public MarketEvent(EventType eventType)
        {
            EventType = eventType;
        }
    }
}
