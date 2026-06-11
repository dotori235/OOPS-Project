using UnityEngine;

public class RoundUIView_CurrentAP : TextUIView
{
    protected override void Awake()
    {
        base.Awake();
        Prefix = "Current AP: ";
    }
}
