using System.Collections.Generic;
using UnityEngine;
using Backend;
using System;
using UnityEngine.UI;
public class MachineSelectUI : MachineUIBase, IMachineSelectButtonObserver
{
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform buttonContainer;
    private List<MachineSelectButton> buttons;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        buttons = new List<MachineSelectButton>();
        foreach (MachineType type in Enum.GetValues(typeof(MachineType)))
        {
            GameObject go = Instantiate(buttonPrefab, buttonContainer);

            buttons.Add(go.GetComponent<MachineSelectButton>());
            go.GetComponent<MachineSelectButton>().Initialize(type, this);
        }
        CloseUI();

    }
    /*
    public override void OpenUI(BeltBlock block, GameObject selectObj)
    {

        base.OpenUI(block, selectObj);
    }
    public override void CloseUI(GameObject selectObj)
    {
        base.CloseUI(selectObj);
    }*/
    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnNotify(ISubject subject)
    {
    }
    public void OnButtonSelected(MachineSelectButton button)
    {
        TargetBlock.CreateMachine(button.MachineType);
        CloseUI();
    }
}
