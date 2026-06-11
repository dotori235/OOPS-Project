using UnityEngine;
using TMPro;

public class TextUIView : UIView
{
    private TextMeshProUGUI _text;
    private string prefix;
    private string suffix;
    public string Prefix {  get { return prefix; } protected set { prefix = value; } }
    public string Suffix { get { return suffix; } protected set { suffix = value; } }

    protected virtual void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    public override void SetValue(UIUpdateArgs arg)
    {
        _text.text = prefix + arg.Value.ToString() + suffix;
    }
}
