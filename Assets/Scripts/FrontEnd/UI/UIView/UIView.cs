using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.InteropServices.WindowsRuntime;
public class UIUpdateArgs
{
    public UIUpdateArgs(float value){
        _value = value;
    }


    private float _value;
    public float Value { get { return _value; } private set { _value = value; } }


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

public abstract class UIView : MonoBehaviour
{
    public abstract void SetValue(UIUpdateArgs arg);
}
