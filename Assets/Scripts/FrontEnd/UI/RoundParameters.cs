public class RoundParameters
{
    private int _roundNum;
    private float _timeLimit;
    private float _targetAp;
    private float _currentAp;
    public int roundNum { get { return _roundNum; } }
    public float timeLimit { get { return _timeLimit; } }
    public float targetAp { get { return _targetAp; } }
    public float currentAp { get { return _currentAp; } }
    public RoundParameters(int roundNum, float timeLimit, float targetAp, float currentAp)
    {
        _roundNum = roundNum;
        _timeLimit = timeLimit;
        _targetAp = targetAp;
        _currentAp = currentAp;
    }
}
