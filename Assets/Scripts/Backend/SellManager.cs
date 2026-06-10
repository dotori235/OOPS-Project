using UnityEditor.UIElements;
using UnityEngine;

namespace Backend
{
    public class SellManager : MonoBehaviour, IManager, IGameEventListener
    {
        private float _apMultiplier = 1.5f;
        private float _spMultiplier = 1.0f;
        private EventType _activeEvent = EventType.None;

        public float ApMultiplier { get => _apMultiplier; set => _apMultiplier = value; }
        public float SpMultiplier { get => _spMultiplier; set => _spMultiplier = value; }
        public EventType ActiveEvent { get => _activeEvent; private set => _activeEvent = value; }

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

            float splendorValue = 0f;
            float attackPowerValue = 0f;
            if (item is Item concreteItem)
            {
                splendorValue = concreteItem.Splendor;
                attackPowerValue = concreteItem.AttackPower;
                FactoryStatus.GetInstance().AddBrandPoints(splendorValue);
            }
            item.SellItem();
            EventBus.GetInstance().Publish(new ItemSoldEvent(price, false, splendorValue, attackPowerValue));
        }

        public void ApplyFine(ISellable item, float p)
        {
            float penaltyMoney = -p*2f;
            FactoryStatus.GetInstance().ModifyMoney(penaltyMoney);
            //FactoryStatus.GetInstance().UpdateBankruptcyBar(bankruptcyDelta);

            EventBus.GetInstance().Publish(new ItemSoldEvent(0f, true, 0f, 0f));
        }
    }
}
