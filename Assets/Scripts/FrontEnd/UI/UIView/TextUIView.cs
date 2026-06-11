using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class TextUIView : UIView
{
    private string prefix;
    private string suffix;
    public string Prefix {  get { return prefix; } protected set { prefix = value; } }
    public string Suffix { get { return suffix; } protected set { suffix = value; } }

    public override void SetValue(UIUpdateArgs arg)
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        if(arg is TextUpdateArgs trgs)
        {

        }
        text.text = prefix + arg.Value.ToString() + suffix;
    }
}
