using UnityEngine;

namespace Backend
{
    public class SellManager : MonoBehaviour
    {
        private static SellManager _instance;
        public static SellManager Instance { get => _instance; }
        private void Awake()
        {
            if(_instance== null)_instance = this;
        }

        public void SellItem(ISellable item)
        {
            if (item == null) return;

            

            float splendorMult = FactoryStatus.GetInstance().SplendorMultiplier;
            float price = item.CalculatePrice(splendorMult);
            if (item.IsDefective)
            {
                ApplyFine(item, price);
                item.SellItem();
                return;
            }
            FactoryStatus.GetInstance().ModifyMoney(price);

            Stat stats = new Stat();
            if (item is IUpgradable upgradable)
            {
                stats = upgradable.GetStats();
                FactoryStatus.GetInstance().AddBrandPoints(stats.Get(StatType.Splendor));
            }
            item.SellItem();
            EventBus.GetInstance().Publish(new ItemSoldEvent(price, false, stats));
        }

        public void ApplyFine(ISellable item, float p)
        {
            float penaltyMoney = -p*2f;
            FactoryStatus.GetInstance().ModifyMoney(penaltyMoney);
            //FactoryStatus.GetInstance().UpdateBankruptcyBar(bankruptcyDelta);

            EventBus.GetInstance().Publish(new ItemSoldEvent(0f, true, new Stat()));
        }
    }
}
