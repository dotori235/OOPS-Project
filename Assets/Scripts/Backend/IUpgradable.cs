namespace Backend
{
    public interface IUpgradable
    {
        void Upgrade(StatType stat, float amount);
        Stat GetStats();
    }
}
