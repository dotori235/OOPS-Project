using UnityEngine;

public class RoundUIView_TimeLimit : TextUIView
{
    protected override void Awake()
    {
        base.Awake();
        Prefix = "Time Limit: ";
    }
}
