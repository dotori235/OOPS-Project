namespace Backend
{
    public class BankruptcyEvent : GameEvent
    {
        public float BarLevel { get; private set; }

        public BankruptcyEvent(float barLevel)
        {
            BarLevel = barLevel;
        }
    }
}
