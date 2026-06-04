using UnityEngine;

public class BeltSelect : MonoBehaviour
{
    [SerializeField]private MachineSelectUI m_SelectUI;
    [SerializeField] private MachineModifyUI m_ModifyUI;
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
                    beltBlock.SelectBlock();
                    if(beltBlock.machine == null)
                    {
                        InstallMachine();
                    }
                    else
                    {
                        Debug.Log(beltBlock.machine);
                        //levelup
                    }
                }
            }
        }
    }
    
    private void InstallMachine()
    {
        m_SelectUI.OpenUI();
        m_ModifyUI.gameObject.SetActive(false);
    }

}
