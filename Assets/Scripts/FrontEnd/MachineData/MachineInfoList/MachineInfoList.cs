using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "MachineInfoList", menuName = "Scriptable Objects/MachineInfoList")]
public class MachineInfoList : ScriptableObject
{
    [SerializeField] private List<MachineInfo> machineInfos;
    public int Count {  get { return machineInfos.Count; } }
    public MachineInfo GetMachineAt(int i)
    {
        return machineInfos[i];
    }
    
}
