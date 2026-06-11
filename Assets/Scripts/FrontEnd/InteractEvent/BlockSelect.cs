using Backend;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
public enum BlockUIType
{
    Null, MachineSelect, MachineModify,TrackModify
}
public class BlockSelect : MonoBehaviour
{

    [SerializeField] private GameObject selectObj;
    [System.Serializable]
    public struct UIPanelMapping
    {
        public BlockUIType uiType;
        public UIPanelBase panel;
    }
    [SerializeField] private List<UIPanelMapping> m_UiPanels;
    private Dictionary<BlockUIType, UIPanelBase> uiDict = new Dictionary<BlockUIType, UIPanelBase>();
    private Camera mainCamera;
    private BlockBase currentBlock;
    private int layerMask;

    private void Awake()
    {
        mainCamera = Camera.main;
        layerMask = LayerMask.GetMask("Block");
        foreach(var map in m_UiPanels)
        {
            if (map.panel != null && !uiDict.ContainsKey(map.uiType))
            {
                uiDict.Add(map.uiType, map.panel);
            }
        }
    }

    private void Start()
    {
        selectObj.SetActive(false);
        foreach (var ui in uiDict.Values)
        {
            ui.SetSelectOj(selectObj);
        }

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

        if (!Physics.Raycast(ray, out RaycastHit hit, 100f, layerMask)) {  return; }

        if (!hit.collider.TryGetComponent(out BlockBase clickedBlock)) return;

        if (currentBlock == clickedBlock && selectObj.activeSelf)
        {
            DeselectCurrentBlock();
            return;
        }
        SelectNewBlock(clickedBlock);

        BlockUIType uiType = clickedBlock.UIType();
        SetCurrentUI(uiType);


    }
    private void SetCurrentUI(BlockUIType uiType)
    {
        /*
        foreach(var ui in uiDict.Keys)
        {
            if(ui != uiType)
            {
                uiDict[ui].CloseUI();
            }
        }*/
        CloseAllUI();
        uiDict[uiType].OpenUI(currentBlock);
    }
    private void SelectNewBlock(BlockBase newBlock)
    {
        if (currentBlock != null)
        {
            currentBlock.UnselectBlock();
        }

        currentBlock = newBlock;
        currentBlock.SelectBlock();
    }

    private void DeselectCurrentBlock()
    {
        if (currentBlock != null)
        {
            currentBlock.UnselectBlock();
        }
        currentBlock = null;
        CloseAllUI();
    }

    private void CloseAllUI()
    {
        foreach(UIPanelBase ui in uiDict.Values)
        {
            ui.CloseUI();
        }
    }
}