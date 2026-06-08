using Backend;
using UnityEngine;

public class BeltBlock : MonoBehaviour
{
    private Machine _machine;
    public Machine machine { get { return _machine; } set { _machine = value; } }
    private void Start()
    {
        _machine = null;
    }
    public void SelectBlock()
    {

    }
    public void UnselectBlock()
    {
    }
    public void CreateMachine(MachineType type)
    {
        GameObject go = Instantiate(MachineManager.Instance.GetMachine(type));
        _machine = go.GetComponent<Machine>();
        go.transform.position = transform.position;
    }
    
}
