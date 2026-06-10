using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
public abstract class UIPanelButtonBase : MonoBehaviour, IUIPanelButtonSubject
{
    private List<IObserver> _observers = new List<IObserver>();
    protected virtual void Awake()
    {
        
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }
    public virtual void OnButtonClick()
    {
        NotifyObservers();
    }
    public void RegisterObserver(IObserver observer)
    {
        if (_observers.Contains(observer)) return;
        _observers.Add(observer);
    }
    public void UnregisterObserver(IObserver observer)
    {
        if (!_observers.Contains(observer)) return;

        _observers.Remove(observer);
    }
    public void NotifyObservers()
    {
        foreach (var observer in _observers)
        {
            observer?.OnNotify(this);

            if(observer is IUIPanelButtonObserver mbOb)
            {
                mbOb.OnButtonSelected(this);
            }
        }
    }
}
