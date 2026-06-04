using Backend;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class MachineSelectButton : MonoBehaviour, ISubject
{
    [SerializeField] private TextMeshProUGUI text;

    private MachineType machineType;
    private List<IObserver> _observers;
    public MachineType MachineType {  get { return machineType; } set { machineType = value; } }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _observers = new List<IObserver>();
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(OnMachineSelectClick);
    }
    public void Initialize(MachineType type, IMachineSelectButtonObserver observer)
    {
        machineType = type;
        text.text = machineType.ToString();
        RegisterObserver(observer);
    }

    public void OnMachineSelectClick()
    {
        NotifyObservers();
    }
    public void RegisterObserver(IObserver observer) => _observers.Add(observer);
    public void UnregisterObserver(IObserver observer) => _observers.Remove(observer);
    public void NotifyObservers()
    {
        foreach (var observer in _observers)
        {
            observer?.OnNotify(this);

            if (observer is IMachineSelectButtonObserver machineObserver)
            {
                machineObserver.OnButtonSelected(this);
            }
        }
    }
        
}
