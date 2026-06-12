using UnityEngine;

public class MachineModifyUIView_Level : TextUIView
{
    protected override void Awake()
    {
        base.Awake();
        Prefix = "Level: ";
    }
}
