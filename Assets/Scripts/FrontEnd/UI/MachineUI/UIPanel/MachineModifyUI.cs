using Backend;
using UnityEngine;

public class MachineModifyUI : UIPanelBase
{
    [SerializeField] private UIView _machineTypeTxt;
    [SerializeField] private UIView _levelTxt;
    [SerializeField] private UIView _levelUpPayTxt;
    [SerializeField] private MachineModifyButton levelupBtn;
    [SerializeField] private MachineModifyButton sellBtn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        levelupBtn?.RegisterObserver(this);
        sellBtn?.RegisterObserver(this);
        Panel.SetActive(false);
    }
    private void OnDestroy()
    {
        levelupBtn?.UnregisterObserver(this);
        sellBtn?.UnregisterObserver(this);
    }
    public override void OnBlockChanged(IBlockSubject beltBlock)
    {
        base.OnBlockChanged (beltBlock);
        BeltBlock bb = beltBlock as BeltBlock;
        UIUpdateArgs type = new TextUpdateArgs(bb.MachineName);
        UIUpdateArgs level = new TextUpdateArgs(bb.MachineLevel.ToString());
        UIUpdateArgs pay = new TextUpdateArgs(bb.MachineLevelUpPrice.ToString());
        _machineTypeTxt.SetValue(type);
        _levelTxt.SetValue(level);
        _levelUpPayTxt.SetValue(pay);
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
        //CloseUI();
    }


}
