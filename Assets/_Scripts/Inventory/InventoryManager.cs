using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    
    public int MaxSlots = 16;


    
    public List<SlotItemData> slotItemDatas;
    public event Action OnInventoryChanged;

    
    [SerializeField] private List<ItemData> TestItemData;
    private void Start()
    {
        Init();
    }
    private void Init()
    {
        
        
        slotItemDatas= new List<SlotItemData>();
        
        for (int i = 0; i < MaxSlots; i++)
        {
            slotItemDatas.Add(new SlotItemData());
        }
    }

    public bool AddInventoryitme(ItemData itemData)
    {
        
        var emptySlot = slotItemDatas.Find(slot => slot.IsEmpty);
        if (emptySlot != null)
        {
            emptySlot.AddItem(itemData);
            //slotItemDatas.Add(emptySlot);
            OnInventoryChanged?.Invoke(); 
            return true;
        }
        return false;
    }

    public bool RemoveInventoryitme(ItemData itemData)
    {
        
        // item 이 null인데 find를 찾을려고 해서 버그
        // null 검사를 하고 id를 비교해야됨 
        var ExistItme=slotItemDatas.Find(slotitem => 
        slotitem !=null &&
        slotitem.item !=null &&
        slotitem.item.id == itemData.id);
        if (ExistItme !=null)
        {
            ExistItme.RemoveItem(itemData);
            //slotItemDatas.Remove(ExistItme);
            OnInventoryChanged?.Invoke();
            return true;
        }
        return false;
    }


    
    public void Additem_test()
    {
        for (int i = 0; i < TestItemData.Count; i++)
        {
            AddInventoryitme(TestItemData[i]);
        }
    }

    public void Removeitem_test()
    {
        for (int i = 0; i < TestItemData.Count; i++)
        {
            RemoveInventoryitme(TestItemData[i]);
        }
    }



    //public void InventoryItemAdd(ItemData item)
    //{
    //    inventoryItem.Add(item.id, item);
    //}
}
