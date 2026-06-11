using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.InteropServices.WindowsRuntime;
public class UIUpdateArgs
{
    public UIUpdateArgs(object value){
        _value = value;
    }


    private object _value;
    public object Value { get { return _value; } private set { _value = value; } }


}

public class SliderUpdateArgs : UIUpdateArgs
{
    public SliderUpdateArgs(float value, float maxValue) :base(value)
    {
        _maxValue = maxValue;
    }
    private float _maxValue;
    public float MaxValue { get { return _maxValue; } set { _maxValue = value; } }
}
public class TextUpdateArgs : UIUpdateArgs
{
    public TextUpdateArgs(string value):base(value)
    {

    }
    
}

public abstract class UIView : MonoBehaviour
{
    public abstract void SetValue(UIUpdateArgs arg);
}
