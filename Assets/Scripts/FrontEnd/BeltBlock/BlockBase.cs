using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public abstract class BlockBase : MonoBehaviour
{

    public abstract BlockUIType UIType();
    public virtual void SelectBlock()
    {
    }

    public virtual void UnselectBlock() { }


}
