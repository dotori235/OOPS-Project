using UnityEngine;
using Backend;

public class RoundUI : MonoBehaviour, IRoundObserver
{
    [SerializeField] private UIView roundNumTxt;
    [SerializeField] private UIView timeLimitTxt;
    [SerializeField] private UIView targetApTxt;
    [SerializeField] private UIView currentApTxt;
    private void Start()
    {
        RoundManager.Instance.RegisterObserver(this);
    }
    private void OnDestroy()
    {
        RoundManager.Instance.UnregisterObserver(this);
    }
    public void OnNotify(ISubject subject)
    {

    }
    public void OnRoundChanged(IRoundSubject roundSubject, UIUpdateArgs arg)
    {
        if(arg.Value is RoundParameters v)
        {
            roundNumTxt.SetValue(new TextUpdateArgs(v.roundNum.ToString()));
            timeLimitTxt.SetValue(new TextUpdateArgs(Mathf.RoundToInt(v.timeLimit).ToString()));
            targetApTxt.SetValue(new TextUpdateArgs(v.targetAp.ToString()));
            currentApTxt.SetValue(new TextUpdateArgs(Mathf.Round(v.currentAp).ToString()));
        }
    }
}
