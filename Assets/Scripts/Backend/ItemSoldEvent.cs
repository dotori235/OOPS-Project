namespace Backend
{
    public class ItemSoldEvent : GameEvent
    {
        public float Price { get; private set; }
        public bool IsDefective { get; private set; }
        public float Splendor { get; private set; }
        public float AttackPower { get; private set; }

        public ItemSoldEvent(float price, bool isDefective, float splendor, float attackPower)
        {
            Price = price;
            IsDefective = isDefective;
            Splendor = splendor;
            AttackPower = attackPower;
        }
    }
}
