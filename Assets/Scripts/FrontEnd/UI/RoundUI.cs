using UnityEngine;
using TMPro;
using Backend;
public class RoundParameters
{
    private int _roundNum;
    private float _timeLimit;
    private float _targetAp;
    private float _currentAp;
    public int roundNum {  get { return _roundNum; } }
    public float timeLimit {  get { return _timeLimit; } }
    public float targetAp {  get { return _targetAp; } }
    public float currentAp {  get { return _currentAp; } }
    public RoundParameters(int roundNum, float timeLimit, float targetAp, float currentAp)
    {
        _roundNum = roundNum;
        _timeLimit = timeLimit;
        _targetAp = targetAp;
        _currentAp = currentAp;
    }

}
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
