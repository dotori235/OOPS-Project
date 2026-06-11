using UnityEngine;

public class MoneyUIView : TextUIView
{
    private void Awake()
    {
        Prefix = "Maney: ";
        Suffix = "$";
    }
}
