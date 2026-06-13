using Backend;
public interface IMachineCommand
{
    string CommandName { get; }
    float GetPrice(Machine machine);
    bool CanExecute(Machine machine);
    void Execute(Machine machine);
}

public class LevelUpCommand : IMachineCommand
{
    public string CommandName => "LevelUp";
    public float GetPrice(Machine machine) => machine.LevelUpPrice;
    public bool CanExecute(Machine machine) => machine != null && machine.CanLevelUp();
    public void Execute(Machine machine) => machine.LevelUp();
}

public class RepairCommand : IMachineCommand
{
    public string CommandName => "Repair";
    public float GetPrice(Machine machine) => Machine.RepairPrice;
    public bool CanExecute(Machine machine) => machine != null && machine.CanRepair();
    public void Execute(Machine machine) => machine.Repair();
}