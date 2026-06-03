using UnityEngine;

namespace Backend
{
    public class FactoryStatus
    {
        private static FactoryStatus _instance;

        private float _money;
        private int _brandLevel;
        private float _brandPoints;
        private float _splendorMultiplier;
        private float _bankruptcyBar;

        public float Money { get => _money; private set => _money = value; }
        public int BrandLevel { get => _brandLevel; private set => _brandLevel = value; }
        public float BrandPoints { get => _brandPoints; private set => _brandPoints = value; }
        public float SplendorMultiplier { get => _splendorMultiplier; private set => _splendorMultiplier = value; }
        public float BankruptcyBar { get => _bankruptcyBar; private set => _bankruptcyBar = value; }

        private FactoryStatus()
        {
            _money = 1000f;
            _brandLevel = 1;
            _brandPoints = 0f;
            _splendorMultiplier = 1.0f;
            _bankruptcyBar = 0f;
        }

        public static FactoryStatus GetInstance()
        {
            if (_instance == null)
            {
                _instance = new FactoryStatus();
            }
            return _instance;
        }

        public void ModifyMoney(float v)
        {
            _money += v;
        }

        public void AddBrandPoints(float sp)
        {
            _brandPoints += sp;
            float threshold = _brandLevel * 100f;
            while (_brandPoints >= threshold)
            {
                _brandPoints -= threshold;
                _brandLevel++;
                _splendorMultiplier = 1.0f + (_brandLevel - 1) * 0.2f;
                threshold = _brandLevel * 100f;
            }
        }

        public void UpdateBankruptcyBar(float delta)
        {
            _bankruptcyBar += delta;
            if (_bankruptcyBar < 0f) _bankruptcyBar = 0f;
            if (_bankruptcyBar > 100f) _bankruptcyBar = 100f;
        }

        public bool IsGameOver()
        {
            return _bankruptcyBar >= 100f;
        }

        public void ResetStatus()
        {
            _money = 1000f;
            _brandLevel = 1;
            _brandPoints = 0f;
            _splendorMultiplier = 1.0f;
            _bankruptcyBar = 0f;
        }
    }
}
