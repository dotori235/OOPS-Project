using Backend;
using UnityEngine;

public class BeltSelect : MonoBehaviour
{
    [SerializeField]private MachineUIBase m_SelectUI;
    [SerializeField] private MachineUIBase m_ModifyUI;
    [SerializeField] private GameObject selectObj;
    private BeltBlock currentBlock;
    private int layerMask;

    private void Awake()
    {
        layerMask = LayerMask.GetMask("BeltBlock");
    }
    private void Start()
    {
        selectObj.SetActive(false);
        m_SelectUI.SetSelectOj(selectObj);
        m_ModifyUI.SetSelectOj(selectObj);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, layerMask))
            {
                if (hit.collider.gameObject.CompareTag("BeltBlock"))
                {
                    BeltBlock beltBlock = hit.collider.gameObject.GetComponent<BeltBlock>();
                    if(currentBlock == beltBlock)
                    {
                        currentBlock.UnselectBlock();
                        currentBlock = null;

                        m_SelectUI.CloseUI();
                        m_ModifyUI.CloseUI();
                    }
                    else
                    {
                        if(currentBlock != null)
                            currentBlock.UnselectBlock();
                        currentBlock = beltBlock;
                        currentBlock.SelectBlock();
                        if (beltBlock.machine == null)
                        {

                            MachineSelectUIOpen();
                        }
                        else
                        {
                            MachineModifyUIOpen();
                        }
                    }
                }
                else
                {

                }
            }
        }
    }
    
    private void MachineSelectUIOpen()
    {
        m_ModifyUI.CloseUI();
        m_SelectUI.OpenUI(currentBlock);
        
    }
    private void MachineModifyUIOpen()
    {
        m_SelectUI.CloseUI();
        m_ModifyUI.OpenUI(currentBlock);
    }

}
