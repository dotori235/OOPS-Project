using Backend;
using UnityEngine;

public class BeltBlock : MonoBehaviour
{
    private Machine _machine;
    public Machine machine { get { return _machine; } set { _machine = value; } }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _machine = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SelectBlock()
    {
        Debug.Log(transform.position);
    }
}
