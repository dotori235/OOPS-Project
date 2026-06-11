using UnityEngine;

public class TrackModifyUIView_Level : TextUIView
{
    protected override void Awake()
    {
        base.Awake();
        Prefix = "Track Level: ";
    }
}
