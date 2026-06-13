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
        UIUpdateArgs type = new TextUpdateArgs(bb.GetMachineType().ToString());
        UIUpdateArgs level = new TextUpdateArgs(bb.Level.ToString());
        UIUpdateArgs pay = new TextUpdateArgs(bb.LevelUpPrice.ToString());
        _machineTypeTxt.SetValue(type);
        _levelTxt.SetValue(level);
        _levelUpPayTxt.SetValue(pay);
        _hpGauge?.SetValue(new SliderUpdateArgs(bb.HpRatio, 1f));
        _repairPayTxt?.SetValue(new TextUpdateArgs(Machine.RepairPrice.ToString()));
        levelupBtn?.SetInteractable(bb.CanLevelUp());
        repairBtn?.SetInteractable(bb.CanRepair());
    }

    public override void OnButtonSelected(IUIPanelButtonSubject button)
    {
        if(button is MachineModifyButton_Levelup levelupBtn)
        {
            (TargetBlock as BeltBlock).MachineLevelUp();
        }
        if (button is MachineModifyButton_Sell sellBtn)
        {
            (TargetBlock as BeltBlock).SellMachine();
            CloseUI();
        }
        if (button is MachineModifyButton_Repair repairBtn)
        {
            (TargetBlock as BeltBlock).MachineRepair();
        }
        //CloseUI();
    }


}
