using UnityEngine;
using System.Collections.Generic;
using Backend;
public class SellBlock : BlockBase,ISellBlockSubject
{
    private List<IObserver> _observers = new List<IObserver>();
    public override BlockUIType UIType()
    {
        return BlockUIType.Null;
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.TryGetComponent<ISellable>(out ISellable itme))
            NotifySellBlock(itme);
    }
    public void RegisterObserver(IObserver observer)
    {
        if (_observers.Contains(observer)) return;
        _observers.Add(observer);
}
    public void UnregisterObserver(IObserver observer)
    {
        if(!_observers.Contains(observer)) return;
        _observers.Remove(observer);
    }
    public void NotifyObservers()
    {

    }

    public void NotifySellBlock(ISellable item)
    {
        foreach (IObserver observer in _observers)
        {
            if(observer is ISellBlockObserver sbOb)
            {
                sbOb.OnSellBlockReached(item);
            }
        }
    }
}
