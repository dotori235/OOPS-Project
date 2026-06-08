using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class MachineModifyUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CloseUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OpenUI(BeltBlock block)
    {
        panel.SetActive(true);
    }
    public void CloseUI()
    {

        panel.SetActive(false);
    }
}
