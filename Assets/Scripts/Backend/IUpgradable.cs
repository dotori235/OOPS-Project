using System.Collections.Generic;

namespace Backend
{
    public interface IUpgradable
    {
        void Upgrade(StatType stat, float amount);
        Dictionary<StatType, float> GetStats();
    }
}
