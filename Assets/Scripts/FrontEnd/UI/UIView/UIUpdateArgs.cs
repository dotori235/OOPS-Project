public class UIUpdateArgs
{
    public UIUpdateArgs(object value)
    {
        _value = value;
    }

    private object _value;
    public object Value { get { return _value; } private set { _value = value; } }
}
