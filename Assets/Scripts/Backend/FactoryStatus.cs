using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;
public enum Operation { 
    Addition, Multiplication, Assignment
}

namespace Backend
{
    public class FactoryStatus : MonoBehaviour, IFactoryStatusSubject
    {
        private static FactoryStatus _instance;


        private float _splendorMultiplier;

        private Dictionary<FactoryStatusType, float> _factoryStatusValue;
        private List<IObserver> _observers;
        public float Money { get => _factoryStatusValue[FactoryStatusType.Money]; private set => _factoryStatusValue[FactoryStatusType.Money] = value; }
        public float BrandLevel { get => _factoryStatusValue[FactoryStatusType.BrandLevel]; private set => _factoryStatusValue[FactoryStatusType.BrandLevel] = value; }
        public float BrandPoints { get => _factoryStatusValue[FactoryStatusType.BrandPoints]; private set => _factoryStatusValue[FactoryStatusType.BrandPoints] = value; }
        public float SplendorMultiplier { get => _splendorMultiplier; private set => _splendorMultiplier = value; }
        public float BankruptcyBar { get => _factoryStatusValue[FactoryStatusType.BankruptcyBar]; private set => _factoryStatusValue[FactoryStatusType.BankruptcyBar] = value; }
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
            NotifyFactoryStatus(type, _factoryStatusValue[type]);

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
            
        }
        private void Update()
        {
            if (_factoryStatusValue[FactoryStatusType.Money] < 0)
            {
                UpdateBankruptcyBar(-_factoryStatusValue[FactoryStatusType.Money] / 1000 * Time.deltaTime);
            }
        }
        
        public void NotifyFactoryStatus(FactoryStatusType type, float value)
        {
            foreach (var observer in _observers)
            {
                observer?.OnNotify(this);

                if (observer is IFactoryStatusObserver factoryStatusObserver)
                {
                    factoryStatusObserver.OnFactoryStatusChanged(type, value);
                }
            }
        }
        private void Awake()
        {
            _instance = this;
            _observers = new List<IObserver>();
            _factoryStatusValue = new Dictionary<FactoryStatusType, float>();
            foreach(FactoryStatusType type in Enum.GetValues(typeof(FactoryStatusType)))
            {
                _factoryStatusValue.Add(type, 0);
            }

        }
        private void Start()
        {
            ResetStatus();

        }
        /*
        private FactoryStatus()
        {
            _money = 1000f;
            _brandLevel = 1;
            _brandPoints = 0f;
            _splendorMultiplier = 1.0f;
            _bankruptcyBar = 0f;
        }
        */
        public static FactoryStatus GetInstance()
        {
            
            return _instance;
        }

        public void ModifyMoney(float v)
        {
            SetValue(FactoryStatusType.Money, Operation.Addition, v);
            /*
            _factoryStatusValue[FactoryStatusType.Money] += v;
            NotifyFactoryStatus(FactoryStatusType.Money, _factoryStatusValue[FactoryStatusType.Money]);*/
        }

        public void AddBrandPoints(float sp)
        {
            SetValue(FactoryStatusType.BrandPoints, Operation.Addition, sp);
            float threshold = _factoryStatusValue[FactoryStatusType.BrandLevel] * 100f;
            while (_factoryStatusValue[FactoryStatusType.BrandPoints] >= threshold)
            {
                SetValue(FactoryStatusType.BrandPoints, Operation.Addition, -threshold);
                SetValue(FactoryStatusType.BrandLevel, Operation.Addition, 1);
                _splendorMultiplier = 1.0f + (_factoryStatusValue[FactoryStatusType.BrandLevel] - 1) * 0.2f;
                threshold = _factoryStatusValue[FactoryStatusType.BrandLevel] * 100f;


            }

        }

        public void UpdateBankruptcyBar(float delta)
        {
            SetValue(FactoryStatusType.BankruptcyBar, Operation.Addition, delta);
            if (_factoryStatusValue[FactoryStatusType.BankruptcyBar] < 0f) _factoryStatusValue[FactoryStatusType.BankruptcyBar] = 0f;
            if (_factoryStatusValue[FactoryStatusType.BankruptcyBar] > 1f) _factoryStatusValue[FactoryStatusType.BankruptcyBar] = 1f;
        }

        public bool IsGameOver()
        {
            return _factoryStatusValue[FactoryStatusType.BankruptcyBar] >= 1f;
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
