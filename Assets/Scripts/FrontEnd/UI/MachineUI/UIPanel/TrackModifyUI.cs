using Backend;
using UnityEngine;

public class TrackModifyUI : UIPanelBase, IBeltTrackLevelObserver
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
    public override void OpenUI(BlockBase block)
    {
        base.OpenUI(block);
        if(block is TrackBlock tb)
        {
            tb.BeltTrack.RegisterObserver(this);
        }
    }
    public override void CloseUI()
    {
        if(TargetBlock is TrackBlock tb)
        {
            tb.BeltTrack.UnregisterObserver(this);
        }
        base.CloseUI();
    }

public void OnBeltTrackLevelChanged(IBeltTrackLevelSubject belt)
    {
        BeltTrack trackBlock = belt as BeltTrack;
        UIUpdateArgs level = new TextUpdateArgs(trackBlock.Level.ToString());
        UIUpdateArgs pay = new TextUpdateArgs(trackBlock.LevelUpPrice.ToString());
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
