using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeNewItem : ItemClass
{
    public override bool IsGrab()
    {
        return true;
    }
    public override void InteractItem()
    {
        PickItem();
    }
}
