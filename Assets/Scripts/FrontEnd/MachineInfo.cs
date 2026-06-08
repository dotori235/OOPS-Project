using Backend;
using UnityEngine;

[CreateAssetMenu(fileName = "MachineInfo", menuName = "Scriptable Objects/MachineInfo")]
public class MachineInfo : ScriptableObject
{
    [SerializeField] private MachineType type;
    [SerializeField] private GameObject machine;
    public MachineType GetMachineType()
    {
        return type;
    }
    public GameObject GetMachine() { 
        return machine;
    }
}
