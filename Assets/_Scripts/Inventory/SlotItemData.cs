using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotItemData 
{
    public ItemData item;
    public int amount;

    public bool IsEmpty => item == null;
    //public bool IsEmpty;

    public SlotItemData()
    {
        item = null;
        amount = 0;
        //IsEmpty= true;
    }

    public void AddItem(ItemData newitem, int count = 1)
    {
        item = newitem;
        amount = count;
        //IsEmpty= false;
    }

    public void RemoveItem(ItemData newitem, int count = 1)
    {
        item = null;
        amount -= count;
        //IsEmpty= true;
        if (amount <= 0)
        {
            amount = 0;
        }
    }
}
