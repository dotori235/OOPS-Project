using System.Collections.Generic;
using UnityEngine;
using Backend;
using System;
using UnityEngine.UI;
public class MachineSelectUI : MachineUIBase, IMachineButtonObserver
{
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform buttonContainer;
    private List<MachineButtonBase> buttons = new List<MachineButtonBase>();
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

    public void OnButtonSelected(IMachineButtonSubject button)
    {
        if(button is MachineSelectButton msB && TargetBlock)
            (TargetBlock as BeltBlock).CreateMachine(msB.MachineType);
        CloseUI();
    }
}
