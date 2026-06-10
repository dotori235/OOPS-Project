using UnityEngine;
using System.Collections.Generic;
using Backend;
public class SellBlock : BlockBase,ISellBlockSubject
{
    private void OnTriggerEnter(Collider other)
    {

        if (other.TryGetComponent<ISellable>(out ISellable itme))
            NotifySellBlock(itme);
    }


    public void NotifySellBlock(ISellable item)
    {
        foreach (IObserver observer in Observers)
        {
            if(observer is ISellBlockObserver sbOb)
            {
                sbOb.OnSellBlockReached(item);
            }
        }
    }
}
