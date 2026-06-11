using UnityEngine;

public class BrandLevelUIView : TextUIView
{
    protected override void Awake()
    {
        base.Awake();
        Prefix = "Brand Level: ";
    }
}
