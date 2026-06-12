using UnityEngine;

public class RoundUIView_TargetAP : TextUIView
{
    protected override void Awake()
    {
        base.Awake();
        Prefix = "Target AP: ";
    }
}
