using UnityEngine;
using System.Collections.Generic;
using Backend;
public class SellBlock : BlockBase,ISellBlockSubject
{
    public override BlockUIType UIType()
    {
        return BlockUIType.Null;
    }
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
