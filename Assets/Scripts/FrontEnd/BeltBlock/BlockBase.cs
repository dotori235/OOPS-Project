using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public abstract class BlockBase : MonoBehaviour, IBlockSubject
{
    private List<IObserver> _observers = new List<IObserver>();
    protected List<IObserver> Observers { get=>_observers ; private set=>_observers=value; }
    public abstract BlockUIType UIType();
    public virtual void SelectBlock()
    {
        StartCoroutine(NotifyDelay());
    }
    private IEnumerator NotifyDelay()
    {
        yield return null;
        NotifyBlock();
    }
    public virtual void UnselectBlock() { }
    public virtual void RegisterObserver(IObserver observer)
    {
        if (_observers.Contains(observer)) return;
        _observers.Add(observer);
    }
    public virtual void UnregisterObserver(IObserver observer)
    {
        if (!_observers.Contains(observer)) return;
        _observers.Remove(observer);
    }
    public virtual  void NotifyBlock()
    {
        foreach (var observer in _observers)
        {

            if (observer is IBlockObserver beltBlockObserver)
            {
                beltBlockObserver.OnBlockChanged(this);
            }
        }
    }
    public void NotifyObservers()
    {

    }

}
