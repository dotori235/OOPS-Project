using System.Collections.Generic;
using UnityEngine;
using Backend;
using System;
using UnityEngine.UI;
public class MachineSelectUI : UIPanelBase
{
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform buttonContainer;
    private List<UIPanelButtonBase> buttons = new List<UIPanelButtonBase>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        
        foreach (MachineType type in Enum.GetValues(typeof(MachineType)))
        {
            GameObject go = Instantiate(buttonPrefab, buttonContainer);

            buttons.Add(go.GetComponent<MachineSelectButton>());
            go.GetComponent<MachineSelectButton>().Initialize(type, this);
        }
        Panel.SetActive(false);

    }

    public override void OnButtonSelected(IUIPanelButtonSubject button)
    {
        if(button is MachineSelectButton msB && TargetBlock)
            (TargetBlock as BeltBlock).CreateMachine(msB.MachineType);
        CloseUI();
    }
}
