using Backend;
using UnityEngine;

public class MachineModifyUI : UIPanelBase, IMachineObserver
{
    [SerializeField] private UIView _machineTypeTxt;
    [SerializeField] private UIView _levelTxt;
    [SerializeField] private UIView _levelUpPayTxt;
    [SerializeField] private UIView _hpGauge;
    [SerializeField] private UIView _repairPayTxt;
    [SerializeField] private MachineModifyButton levelupBtn;
    [SerializeField] private MachineModifyButton sellBtn;
    [SerializeField] private MachineModifyButton repairBtn;
    private readonly IMachineCommand _levelUpCmd = new LevelUpCommand();
    private readonly IMachineCommand _repairCmd = new RepairCommand();
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        levelupBtn?.RegisterObserver(this);
        sellBtn?.RegisterObserver(this);
        repairBtn?.RegisterObserver(this);
        Panel.SetActive(false);
    }
    public override void OpenUI(BlockBase block)
    {
        base.OpenUI(block);
        if(block is BeltBlock bb)
        {
            bb.Machine.RegisterObserver(this);
        }
    }
    public override void CloseUI()
    {
        if(TargetBlock is BeltBlock bb && bb.Machine != null)
        {

            bb.Machine.UnregisterObserver(this);
        }
        base.CloseUI();
    }
    private void OnDestroy()
    {
        levelupBtn?.UnregisterObserver(this);
        sellBtn?.UnregisterObserver(this);
        repairBtn?.UnregisterObserver(this);
    }
    public void OnMachineChanged(IMachineSubject machine)
    {
        Machine bb = machine as Machine;
        if (bb == null) return;

        _machineTypeTxt.SetValue(new TextUpdateArgs(bb.GetMachineType().ToString()));
        _levelTxt.SetValue(new TextUpdateArgs(bb.Level.ToString()));
        _hpGauge?.SetValue(new SliderUpdateArgs(bb.HpRatio, 1f));

        _levelUpPayTxt.SetValue(new TextUpdateArgs(_levelUpCmd.GetPrice(bb).ToString()));
        _repairPayTxt?.SetValue(new TextUpdateArgs(_repairCmd.GetPrice(bb).ToString()));

        levelupBtn?.SetInteractable(_levelUpCmd.CanExecute(bb));
        repairBtn?.SetInteractable(_repairCmd.CanExecute(bb));


    }

    public override void OnButtonSelected(IUIPanelButtonSubject button)
    {
        if(button is MachineModifyButton_Levelup levelupBtn)
        {
            (TargetBlock as BeltBlock).ExecuteCommand(_levelUpCmd);
        }
        if (button is MachineModifyButton_Sell sellBtn)
        {
            (TargetBlock as BeltBlock).SellMachine();
            CloseUI();
        }
        if (button is MachineModifyButton_Repair repairBtn)
        {
            (TargetBlock as BeltBlock).ExecuteCommand(_repairCmd);
        }
        //CloseUI();
    }


}
