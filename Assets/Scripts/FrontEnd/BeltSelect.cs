using Backend;
using UnityEngine;

public class BeltSelect : MonoBehaviour
{
    [SerializeField]private MachineSelectUI m_SelectUI;
    [SerializeField] private MachineModifyUI m_ModifyUI;
    private BeltBlock currentBlock;
    private int layerMask;

    private void Awake()
    {
        layerMask = LayerMask.GetMask("BeltBlock");
    }


    // Update is called once per frame
    void Update()
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
                            Debug.Log(beltBlock.machine);
                            //levelup
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
        m_SelectUI.OpenUI(currentBlock);
        m_ModifyUI.CloseUI();
    }
    private void MachineModifyUIOpen()
    {
        m_SelectUI.CloseUI();
        m_ModifyUI.OpenUI(currentBlock);
    }
    
    public void CreateMachine()
    {

    }
}
