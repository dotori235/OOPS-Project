using Backend;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BeltBlock : BlockBase
{
    private Machine _machine;
    public Machine Machine { get { return _machine; } set { _machine = value; } }
    private FactoryStatus _factoryStatus;
    public string MachineName { get { return _machine == null ? null : _machine.GetMachineType().ToString(); } }
    public float MachineLevel { get => _machine == null ? 0 : _machine.Level; }

    private void Start()
    {
        _machine = null;
        _factoryStatus = FactoryStatus.GetInstance();
    }

    public bool CreateMachine(MachineType type)
    {
        float pay = Machine.InstallPrice;
        if (payMoney(pay))
        {
            GameObject go = Instantiate(MachineManager.Instance.GetMachine(type));
            _machine = go.GetComponent<Machine>();
            go.transform.position = transform.position;
            NotifyBlock();
            return true;
        }
        return false;
    }
    public void SellMachine()
    {
        float price = CalculateSellPrice();
        _factoryStatus.ModifyMoney(price);
        Destroy( _machine.gameObject );
        _machine = null;
    }
    public bool MachineLevelUp()
    {
        float pay = CalculateLevelUpPrice();
        if (payMoney(pay))
        {
            _machine.LevelUp();
            NotifyBlock();
            return true;
        }
        return false;
    }
    private bool payMoney(float pay)
    {
        if (_factoryStatus.Money >= pay)
        {
            _factoryStatus.ModifyMoney(-pay);
            return true;
        }
        return false;
    }
    public float CalculateLevelUpPrice()
    {
        return _machine.Level * Machine.LevelUpPriceCoeff;
    }
    public float CalculateSellPrice()
    {
        return (Machine.InstallPrice + (_machine.Level * (_machine.Level - 1) / 2f) * Machine.LevelUpPriceCoeff) / 2;

    }

    


}
