public class SliderUpdateArgs : UIUpdateArgs
{
    public SliderUpdateArgs(float value, float maxValue) : base(value)
    {
        _maxValue = maxValue;
    }

    private float _maxValue;
    public float MaxValue { get { return _maxValue; } set { _maxValue = value; } }
}
