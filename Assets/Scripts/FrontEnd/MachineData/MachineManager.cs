using Backend;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MachineManager : MonoBehaviour
{
    [SerializeField] private MachineInfoList machineInfoList;
    private Dictionary<MachineType, GameObject> machineDict;
    public static MachineManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        machineDict = new Dictionary<MachineType, GameObject>();
        for (int i = 0; i < machineInfoList.Count; i++) { 
            MachineInfo machineInfo = machineInfoList.GetMachineAt(i);
            machineDict.Add(machineInfo.GetMachineType(), machineInfo.GetMachine());
        }

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public GameObject GetMachine(MachineType type)
    {
        return machineDict[type];
    }
}
