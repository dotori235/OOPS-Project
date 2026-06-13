using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
public abstract class UIPanelButtonBase : MonoBehaviour, IUIPanelButtonSubject
{
    private List<IObserver> _observers = new List<IObserver>();
    private Button _button;
    protected virtual void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(OnButtonClick);
    }
    public void SetInteractable(bool value)
    {
        _button.interactable = value;
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
