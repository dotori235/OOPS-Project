using UnityEngine;

public class RoundUIView_RoundNum : TextUIView
{
    protected override void Awake()
    {
        base.Awake();
        Prefix = "Round ";
    }
}
