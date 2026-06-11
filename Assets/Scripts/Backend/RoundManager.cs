using System.Collections.Generic;
using UnityEngine;

namespace Backend
{
    public class RoundManager : MonoBehaviour, IManager, IGameEventListener, IRoundSubject
    {
        private int _roundNumber = 1;
        private float _apThreshold = 15f;
        private float _checkInterval = 30f;
        private float _bonus = 500f;
        private float _penalty = 20f;
        private List<IObserver> _observers = new List<IObserver>();
        private float _timer = 0f;
        private float _observerNotifyInterval = 1;
        private float _observerNotifyTimer = 0f;
        private readonly List<float> _soldItemAPs = new List<float>();
        private static RoundManager instance;
        public static RoundManager Instance { get => instance;}
        public int RoundNumber { get => _roundNumber; private set => _roundNumber = value; }
        public float ApThreshold { get => _apThreshold; private set => _apThreshold = value; }
        public float CheckInterval { get => _checkInterval; private set => _checkInterval = value; }
        public float Bonus { get => _bonus; private set => _bonus = value; }
        public float Penalty { get => _penalty; private set => _penalty = value; }
        public float AverageAP { get { if (_soldItemAPs.Count == 0) { return 0; } float sum = 0;  foreach(float x in _soldItemAPs) {  sum += x; } return sum/_soldItemAPs.Count;  }  }

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
        }
        private void Start()
        {
            EventBus.GetInstance().Subscribe(this);
            StartRound();
        }

        private void OnDestroy()
        {
            EventBus.GetInstance().Unsubscribe(this);
        }

        private void Update()
        {
            _observerNotifyTimer += Time.deltaTime;
            
            _timer += Time.deltaTime;
            if (_timer >= _checkInterval)
            {
                EndRound();
            }
            if(_observerNotifyTimer >= _observerNotifyInterval)
            {
                _observerNotifyTimer = 0;
                NotifyRound();
            }
        }
        
        public void OnEvent(GameEvent e)
        {
            if (e is ItemSoldEvent soldEvent)
            {
                if (!soldEvent.IsDefective)
                {
                    _soldItemAPs.Add(soldEvent.AttackPower);
                    if (_soldItemAPs.Count >= 5)
                    {
                        _soldItemAPs.RemoveAt(0);
                    }
                    NotifyRound();
                }
            }
        }

        public void StartRound()
        {
            _timer = 0f;
            _soldItemAPs.Clear();
            Debug.Log($"[RoundManager] Round {_roundNumber} started! Target AP: {_apThreshold}");
            NotifyRound();
        }

        private void EndRound()
        {
            float avgAP = AverageAP;

            bool passed = avgAP >= _apThreshold;

            if (passed)
            {
                Debug.Log($"[RoundManager] Round {_roundNumber} passed! Avg AP: {avgAP} >= Threshold: {_apThreshold}. Reward: +{_bonus}");
                FactoryStatus.GetInstance().ModifyMoney(_bonus);
            }
            else
            {
                Debug.Log($"[RoundManager] Round {_roundNumber} failed! Avg AP: {avgAP} < Threshold: {_apThreshold}. Penalty: +{_penalty} Bankruptcy Bar");
                //FactoryStatus.GetInstance().UpdateBankruptcyBar(_penalty);
                FactoryStatus.GetInstance().ModifyMoney(-_bonus * 0.5f);
            }

            EventBus.GetInstance().Publish(new RoundEndEvent(avgAP, passed));

            ScaleNextRound();
            StartRound();
        }

        public void ScaleNextRound()
        {
            _roundNumber++;
            _apThreshold += 5f;
            _bonus += 200f;
        }

        public void RegisterObserver(IObserver observer)
        {
            if (_observers.Contains(observer)) return;
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
        public void NotifyRound()
        {
            foreach (var observer in _observers) { 
                if(observer is IRoundObserver rOb){
                    UIUpdateArgs arg = new UIUpdateArgs(new RoundParameters(_roundNumber, _checkInterval-_timer, _apThreshold, AverageAP));
                    rOb.OnRoundChanged(this, arg);
                }
            }
        }
    }
}
