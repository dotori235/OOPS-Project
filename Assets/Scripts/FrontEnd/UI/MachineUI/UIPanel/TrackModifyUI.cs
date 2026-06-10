using UnityEngine;

public class TrackModifyUI : UIPanelBase
{
    [SerializeField] private UIView _trackLevelTxt;
    [SerializeField] private UIView _trackLevelUpPriceTxt;
    [SerializeField] private UIPanelButtonBase _trackLevelUpBtn;
    private void Start()
    {
        _trackLevelUpBtn?.RegisterObserver(this);
        Panel.SetActive(false);
    }
    private void OnDestroy()
    {
        _trackLevelUpBtn?.UnregisterObserver(this);
    }
    public override void OnBlockChanged(IBlockSubject beltBlock)
    {
        base.OnBlockChanged(beltBlock);
        
        TrackBlock trackBlock = beltBlock as TrackBlock;
        UIUpdateArgs level = new TextUpdateArgs(trackBlock.TrackLevel.ToString());
        UIUpdateArgs pay = new TextUpdateArgs(trackBlock.TrackLevelUpPrice.ToString());
        _trackLevelTxt.SetValue(level);
        _trackLevelUpPriceTxt.SetValue(pay);
    }
    public override void OnButtonSelected(IUIPanelButtonSubject button)
    {
        if(button is TrackModifyButton_LevelUp tlBtn)
        {
            (TargetBlock as TrackBlock).TrackLevelUp();
        }
    }
}
