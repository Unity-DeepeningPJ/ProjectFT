using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotItem : MonoBehaviour
{
    public ItemData item;
    public int amount;

    public bool IsEmpty => item == null;

    public SlotItem()
    {
        item = null;
        amount = 0;
    }

    public void AddItem(ItemData newitem, int count = 1)
    {
        item = newitem;
        amount = count;
    }

    public void RemoveItem(ItemData newitem, int count = 1)
    {
        amount -= count;
        if (amount <= 0)
        {
            item = null;
            amount = 0;
        }
    }
}
