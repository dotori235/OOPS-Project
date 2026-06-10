using System;
using UnityEngine;

public class MachineUIBase : MonoBehaviour, IBlockObserver
{
    [SerializeField] private GameObject panel;
    private BlockBase _targetBlock;
    private GameObject selectObj;
    public BlockBase TargetBlock {  get { return _targetBlock; } protected set { _targetBlock = value; }  }
    protected GameObject Panel { get => panel; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public virtual void OpenUI(BlockBase block)
    {
        _targetBlock = block;
        _targetBlock?.RegisterObserver(this);
        selectObj.SetActive(true);
        selectObj.transform.position = _targetBlock.transform.position;
        panel.SetActive(true);
    }
    public virtual void CloseUI()
    {
        if(_targetBlock!=null)
        {
            _targetBlock?.UnregisterObserver(this);

        }

        _targetBlock = null;
        panel.SetActive(false);
        if (selectObj != null)
            selectObj.SetActive(false);
    }
    public virtual void SetSelectOj(GameObject selectObj)
    {
        this.selectObj = selectObj;
    }
    public virtual void OnNotify(ISubject subject)
    {

    }
    public virtual void OnBlockChanged(IBlockSubject beltBlock)
    {

    }
}
