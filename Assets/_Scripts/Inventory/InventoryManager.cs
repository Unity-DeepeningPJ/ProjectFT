using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // 인벤토리가 변했을때 다시 초기화 해주는 이벤트도 만들어야됨 
    public int MaxSlots = 16;


    // 인벤토리 보유 아이템list 
    //public Dictionary<int, ItemData> inventoryItem;
    //인벤토리 슬롯Data list 
    public List<SlotItemData> slotItemDatas;
    public event Action OnInventoryChanged;



    private void Init()
    {
        //아이템 데이터
        //inventoryItem = new Dictionary<int, ItemData>();
        slotItemDatas= new List<SlotItemData>();
        //굳이 개수만큼 만들어줘야하는가?
        for (int i = 0; i < MaxSlots; i++)
        {
            slotItemDatas.Add(new SlotItemData());
        }
    }

    public bool AddInventoryitme(ItemData itemData)
    {
        //slotItemDatas에 아이템 추가 
        var emptySlot = slotItemDatas.Find(slot => slot.IsEmpty);
        if (emptySlot == null)
        {
            emptySlot.AddItem(itemData);
            OnInventoryChanged?.Invoke(); //변경 이벤트 실행
            return true;
        }
        return false;
    }

    public bool RemoveInventoryitme(ItemData itemData)
    {
        //slotItemDatas에서 아이템 삭제 

        var ExistItme=slotItemDatas.Find(slotitem => slotitem.item.id == itemData.id);
        if (ExistItme !=null)
        {
            ExistItme.RemoveItem(itemData);
            OnInventoryChanged?.Invoke();
            return true;
        }
        return false;
    }





    //public void InventoryItemAdd(ItemData item)
    //{
    //    inventoryItem.Add(item.id, item);
    //}
}
