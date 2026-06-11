namespace Backend
{
    public class ItemSoldEvent : GameEvent
    {
        public float Price { get; private set; }
        public bool IsDefective { get; private set; }
        public Stat Stats { get; private set; }

        public ItemSoldEvent(float price, bool isDefective, Stat stats)
        {
            Price = price;
            IsDefective = isDefective;
            Stats = stats ?? new Stat();
        }
    }
}
