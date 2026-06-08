using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class MachineModifyUI : MachineUIBase
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        CloseUI();
    }

    /*
    // Update is called once per frame
    public override void OpenUI(BeltBlock block, GameObject selectObj)
    {

        base.OpenUI(block, selectObj);
    }
    public override void CloseUI(GameObject selectObj)
    {
        base.CloseUI(selectObj);
    }
    */
}
