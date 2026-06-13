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
    public float MachineLevelUpPrice { get => _machine.Level * Machine.LevelUpPriceCoeff; }
    public float MachineSellPrice { get => (Machine.InstallPrice + (_machine.Level * (_machine.Level - 1) / 2f) * Machine.LevelUpPriceCoeff) / 2; }
    public float MachineRepairPrice { get => Machine.RepairPrice; }
    public float MachineHpRatio { get => _machine == null ? 0 : _machine.HpRatio; }
    public bool MachineCanLevelUp { get => _machine != null && _machine.CanLevelUp(); }
    public bool MachineCanRepair { get => _machine != null && _machine.CanRepair(); }

    public override BlockUIType UIType()
    {
        if(_machine == null)
        {
            return BlockUIType.MachineSelect;
        }
        else
        {
            return BlockUIType.MachineModify;
        }
    }
    private void Start()
    {
        _machine = null;
        _factoryStatus = FactoryStatus.GetInstance();
    }

    public bool CreateMachine(MachineType type)
    {
        float pay = Machine.InstallPrice;
        if (PayMoney(pay))
        {
            GameObject go = Instantiate(MachineManager.Instance.GetMachine(type));
            _machine = go.GetComponent<Machine>();
            go.transform.position = transform.position;
            return true;
        }
        return false;
    }
    public void SellMachine()
    {
        float price = MachineSellPrice;
        _factoryStatus.ModifyMoney(price);
        Destroy( _machine.gameObject );
        _machine = null;
    }
    public bool MachineLevelUp()
    {
        // A worn machine must be repaired before leveling up — don't charge for a no-op.
        if (!_machine.CanLevelUp()) return false;

        float pay = MachineLevelUpPrice;
        if (PayMoney(pay))
        {
            _machine.LevelUp();
            return true;
        }
        return false;
    }
    public bool MachineRepair()
    {
        // Don't charge to "repair" a machine that is already at full HP.
        if (!_machine.CanRepair()) return false;

        float pay = MachineRepairPrice;
        if (PayMoney(pay))
        {
            _machine.Repair();
            return true;
        }
        return false;
    }
    private bool PayMoney(float pay)
    {
        if (_factoryStatus.Money >= pay)
        {
            _factoryStatus.ModifyMoney(-pay);
            return true;
        }
        return false;
    }
    /*
    public float CalculateLevelUpPrice()
    {
        return _machine.Level * Machine.LevelUpPriceCoeff;
    }
    public float CalculateSellPrice()
    {
        return (Machine.InstallPrice + (_machine.Level * (_machine.Level - 1) / 2f) * Machine.LevelUpPriceCoeff) / 2;

    }

    */


}
