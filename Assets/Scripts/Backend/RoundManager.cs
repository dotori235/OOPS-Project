using System.Collections.Generic;
using UnityEngine;

namespace Backend
{
    public class RoundManager : MonoBehaviour, IManager, IGameEventListener
    {
        private int _roundNumber = 1;
        private float _apThreshold = 15f;
        private float _checkInterval = 30f;
        private float _bonus = 500f;
        private float _penalty = 20f;

        private float _timer = 0f;
        private readonly List<float> _soldItemAPs = new List<float>();

        public int RoundNumber { get => _roundNumber; private set => _roundNumber = value; }
        public float ApThreshold { get => _apThreshold; private set => _apThreshold = value; }
        public float CheckInterval { get => _checkInterval; private set => _checkInterval = value; }
        public float Bonus { get => _bonus; private set => _bonus = value; }
        public float Penalty { get => _penalty; private set => _penalty = value; }

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
            _timer += Time.deltaTime;
            if (_timer >= _checkInterval)
            {
                EndRound();
            }
        }

        public void OnEvent(GameEvent e)
        {
            if (e is ItemSoldEvent soldEvent)
            {
                if (!soldEvent.IsDefective)
                {
                    _soldItemAPs.Add(soldEvent.AttackPower);
                }
            }
        }

        public void StartRound()
        {
            _timer = 0f;
            _soldItemAPs.Clear();
            Debug.Log($"[RoundManager] Round {_roundNumber} started! Target AP: {_apThreshold}");
        }

        private void EndRound()
        {
            float avgAP = 0f;
            if (_soldItemAPs.Count > 0)
            {
                float sum = 0f;
                foreach (var ap in _soldItemAPs)
                {
                    sum += ap;
                }
                avgAP = sum / _soldItemAPs.Count;
            }

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
    }
}
