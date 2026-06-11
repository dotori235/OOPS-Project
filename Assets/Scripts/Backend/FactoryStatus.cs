using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

namespace Backend
{
    public class FactoryStatus : MonoBehaviour, IFactoryStatusSubject
    {
        private static FactoryStatus _instance;


        private float _splendorMultiplier;

        private Dictionary<FactoryStatusType, float> _factoryStatusValue = new Dictionary<FactoryStatusType, float>();
        private List<IObserver> _observers  = new List<IObserver>();

        public float Money { get => _factoryStatusValue[FactoryStatusType.Money]; private set => _factoryStatusValue[FactoryStatusType.Money] = value; }
        public float BrandLevel { get => _factoryStatusValue[FactoryStatusType.BrandLevel]; private set => _factoryStatusValue[FactoryStatusType.BrandLevel] = value; }
        public float BrandPoints { get => _factoryStatusValue[FactoryStatusType.BrandPoints]; private set => _factoryStatusValue[FactoryStatusType.BrandPoints] = value; }
        public float SplendorMultiplier { get => (BrandLevel - 1) * 0.2f; private set => _splendorMultiplier = value; }
        public float BankruptcyBar { get => _factoryStatusValue[FactoryStatusType.BankruptcyBar]; private set => _factoryStatusValue[FactoryStatusType.BankruptcyBar] = value; }
        public float Threshold()
        {
            return 50 + (BrandLevel - 1) * 100;
        }
        private void SetValue(FactoryStatusType type, Operation op, float value)
        {
            switch (op)
            {
                case Operation.Assignment:
                    _factoryStatusValue[type] = value;
                    break;
                case Operation.Addition:
                    _factoryStatusValue[type] += value;
                    break;
                case Operation.Multiplication:
                    _factoryStatusValue[type] *= value;
                    break;
            }

            UIUpdateArgs arg = new UIUpdateArgs(_factoryStatusValue[type]);
            NotifyFactoryStatus(type, arg);

        }
        public void RegisterObserver(IObserver observer)
        {
            if(_observers.Contains(observer)) return;
            _observers.Add(observer);
        }
        public void UnregisterObserver(IObserver observer)
        {
            if (!_observers.Contains(observer)) return;
            _observers.Remove(observer);
        }
        public void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer?.OnNotify(this);
            }
        }
        private void Update()
        {
            if (Money < 0)
            {
                UpdateBankruptcyBar(-Money / 2000 * Time.deltaTime);
            }
            else
            {
                UpdateBankruptcyBar(-Time.deltaTime/60);
            }
        }
        
        public void NotifyFactoryStatus(FactoryStatusType type, UIUpdateArgs arg)
        {
            foreach (var observer in _observers)
            {

                if (observer is IFactoryStatusObserver factoryStatusObserver)
                {
                    factoryStatusObserver.OnFactoryStatusChanged(type, arg);
                }
            }
        }
        private void Awake()
        {
            _instance = this;
            
            foreach(FactoryStatusType type in Enum.GetValues(typeof(FactoryStatusType)))
            {
                _factoryStatusValue.Add(type, 0);
            }
        }
        private void Start()
        {
            StartCoroutine(ResetDelay());
        
        }
        private IEnumerator ResetDelay()
        {
            yield return null;
            ResetStatus();
        }

        public static FactoryStatus GetInstance()
        {
            
            return _instance;
        }

        public void ModifyMoney(float v)
        {
            SetValue(FactoryStatusType.Money, Operation.Addition, v);
        }

        public void AddBrandPoints(float sp)
        {
            BrandPoints += sp*3;
            bool isLevelUp = false;
            while (BrandPoints >= Threshold())
            {
                isLevelUp = true;
                BrandPoints -= Threshold();
                BrandLevel += 1;
            }
            if (isLevelUp)
            {
                UIUpdateArgs arg = new UIUpdateArgs(BrandLevel);
                NotifyFactoryStatus(FactoryStatusType.BrandLevel, arg);
            }
            UIUpdateArgs slarg = new SliderUpdateArgs(BrandPoints, Threshold());
            NotifyFactoryStatus(FactoryStatusType.BrandPoints, slarg);
        }

        public void UpdateBankruptcyBar(float delta)
        {
            SetValue(FactoryStatusType.BankruptcyBar, Operation.Addition, delta);
            if (BankruptcyBar < 0f) BankruptcyBar = 0f;
            if (BankruptcyBar > 1f) BankruptcyBar = 1f;
        }

        public bool IsGameOver()
        {
            return BankruptcyBar >= 1f; 
        }

        public void ResetStatus()
        {

            SetValue(FactoryStatusType.Money, Operation.Assignment, 1000);
            SetValue(FactoryStatusType.BrandPoints, Operation.Assignment, 0);
            SetValue(FactoryStatusType.BrandLevel, Operation.Assignment, 1);
            SetValue(FactoryStatusType.BankruptcyBar, Operation.Assignment, 0);


        }
    }
}
