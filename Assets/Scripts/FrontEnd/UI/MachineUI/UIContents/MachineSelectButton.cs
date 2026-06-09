using Backend;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class MachineSelectButton : MachineButtonBase
{
    [SerializeField] private TextMeshProUGUI text;

    private MachineType machineType;
    public MachineType MachineType { get { return machineType; } set { machineType = value; } }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void Initialize(MachineType type, IMachineButtonObserver observer)
    {
        machineType = type;
        text.text = machineType.ToString();
        RegisterObserver(observer);
    }

    
        
}
