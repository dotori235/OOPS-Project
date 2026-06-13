using Backend;
using UnityEngine;

public class BeltBlock : BlockBase
{
    private Machine _machine;
    public Machine Machine { get { return _machine; } set { _machine = value; } }
    private FactoryStatus _factoryStatus;
    public float MachineSellPrice { get => (Machine.InstallPrice + (_machine.Level * (_machine.Level - 1) / 2f) * Machine.LevelUpPriceCoeff) / 2; }

    public override BlockUIType UIType()
    {
        if (_machine == null)
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
        Destroy(_machine.gameObject);
        _machine = null;
    }
    public bool ExecuteCommand(IMachineCommand command)
    {
        if (_machine == null || command == null) return false;
        if (!command.CanExecute(_machine)) return false;

        float pay = command.GetPrice(_machine);
        if (PayMoney(pay))
        {
            command.Execute(_machine);
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
}
