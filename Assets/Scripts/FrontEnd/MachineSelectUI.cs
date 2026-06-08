using System.Collections.Generic;
using UnityEngine;
using Backend;
using System;
using UnityEngine.UI;
public class MachineSelectUI : MonoBehaviour, IMachineSelectButtonObserver
{
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private GameObject panel;
    private List<MachineSelectButton> buttons;
    private BeltBlock targetBlock;
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
    public void OpenUI(BeltBlock block)
    {
        targetBlock = block;
        panel.SetActive(true);
    }
    public void CloseUI()
    {
        targetBlock = null;
        panel.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnNotify(ISubject subject)
    {
    }
    public void OnButtonSelected(MachineSelectButton button)
    {
        targetBlock.CreateMachine(button.MachineType);
        CloseUI();
    }
}
