using UnityEngine;

using UnityEngine.UI;
public class SliderUIView : UIView
{

    public override void SetValue(UIUpdateArgs arg)
    {
        Slider slider = GetComponent<Slider>();
        if (arg is SliderUpdateArgs slarg)
        {
            slider.maxValue = slarg.MaxValue;
        }
        slider.value = arg.Value;
    }
}
