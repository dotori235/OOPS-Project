using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class TextUIView : UIView
{
    public override void SetValue(UIUpdateArgs arg)
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        text.text = arg.Value.ToString();
    }
}
