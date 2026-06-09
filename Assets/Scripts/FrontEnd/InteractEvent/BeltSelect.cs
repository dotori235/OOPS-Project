using Backend;
using UnityEngine;
using UnityEngine.EventSystems;

public class BeltSelect : MonoBehaviour
{
    [SerializeField] private MachineUIBase m_SelectUI;
    [SerializeField] private MachineUIBase m_ModifyUI;

    [SerializeField] private GameObject selectObj;

    private Camera mainCamera;
    private BeltBlock currentBlock;
    private int layerMask;

    private void Awake()
    {
        mainCamera = Camera.main;
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
        if (!Input.GetMouseButtonDown(0)) return;
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;
        HandleSelection();
    }

    private void HandleSelection()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit, 100f, layerMask)) return;

        if (!hit.collider.TryGetComponent(out BeltBlock clickedBlock)) return;

        if (currentBlock == clickedBlock && selectObj.activeSelf)
        {
            DeselectCurrentBlock();
            return;
        }

        SelectNewBlock(clickedBlock);
    }

    private void SelectNewBlock(BeltBlock newBlock)
    {
        if (currentBlock != null)
        {
            currentBlock.UnselectBlock();
        }

        currentBlock = newBlock;
        currentBlock.SelectBlock();

        if (currentBlock.Machine == null)
        {
            OpenMachineSelectUI();
        }
        else
        {
            OpenMachineModifyUI();
        }

    }

    private void DeselectCurrentBlock()
    {
        if (currentBlock != null)
        {
            currentBlock.UnselectBlock();
        }

        currentBlock = null;

        m_SelectUI.CloseUI();
        m_ModifyUI.CloseUI();
    }

    private void OpenMachineSelectUI()
    {
        m_ModifyUI.CloseUI();
        m_SelectUI.OpenUI(currentBlock);
    }

    private void OpenMachineModifyUI()
    {
        m_SelectUI.CloseUI();
        m_ModifyUI.OpenUI(currentBlock);
    }
}