using UnityEngine;

public class MachineUIBase : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    private BeltBlock _targetBlock;
    private GameObject selectObj;
    public BeltBlock TargetBlock {  get { return _targetBlock; } protected set { _targetBlock = value; }  }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public virtual void OpenUI(BeltBlock block)
    {

        _targetBlock = block;
        selectObj.SetActive(true);
        selectObj.transform.position = _targetBlock.transform.position;
        panel.SetActive(true);
    }
    public virtual void CloseUI()
    {
        _targetBlock = null;
        panel.SetActive(false);
        if (selectObj != null)
            selectObj.SetActive(false);
    }
    public virtual void SetSelectOj(GameObject selectObj)
    {
        this.selectObj = selectObj;
    }
}
