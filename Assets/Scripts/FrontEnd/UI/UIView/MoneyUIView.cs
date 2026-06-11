using UnityEngine;

public class MoneyUIView : TextUIView
{
    protected override void Awake()
    {
        base.Awake();
        Prefix = "Maney: ";
        Suffix = "$";
    }
}
