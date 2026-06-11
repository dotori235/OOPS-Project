using UnityEngine;

namespace Backend
{
    public class SellManager : MonoBehaviour, IGameEventListener
    {
        private float _apMultiplier = 1.5f;
        private float _spMultiplier = 1.0f;
        private EventType _activeEvent = EventType.None;
        private static SellManager _instance;
        public static SellManager Instance { get => _instance; }
        public float ApMultiplier { get => _apMultiplier; set => _apMultiplier = value; }
        public float SpMultiplier { get => _spMultiplier; set => _spMultiplier = value; }
        public EventType ActiveEvent { get => _activeEvent; private set => _activeEvent = value; }
        private void Awake()
        {
            if(_instance== null)_instance = this;
        }
        private void Start()
        {
            EventBus.GetInstance().Subscribe(this);
        }

        private void OnDestroy()
        {
            EventBus.GetInstance().Unsubscribe(this);
        }

        public void OnEvent(GameEvent e)
        {
            if (e is MarketEvent marketEvent)
            {
                _activeEvent = marketEvent.EventType;
                if (_activeEvent == EventType.Boom)
                {
                    _spMultiplier = 2.0f;
                }
                else if (_activeEvent == EventType.Recession)
                {
                    _spMultiplier = 0.5f;
                }
                else
                {
                    _spMultiplier = 1.0f;
                }
            }
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
