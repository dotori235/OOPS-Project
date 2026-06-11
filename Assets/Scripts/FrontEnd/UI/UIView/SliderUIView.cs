using UnityEngine;
using UnityEngine.UI;

public class SliderUIView : UIView
{
    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    public override void SetValue(UIUpdateArgs arg)
    {
        if (arg is SliderUpdateArgs slarg)
        {
            _slider.maxValue = slarg.MaxValue;
        }
        _slider.value = (float)arg.Value;
    }
}
