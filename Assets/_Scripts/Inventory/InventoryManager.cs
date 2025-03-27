using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // �κ��丮�� �������� �ٽ� �ʱ�ȭ ���ִ� �̺�Ʈ�� �����ߵ� 
    public int MaxSlots = 16;


    // �κ��丮 ���� ������list 
    //public Dictionary<int, ItemData> inventoryItem;
    //�κ��丮 ����Data list 
    public List<SlotItemData> slotItemDatas;
    public event Action OnInventoryChanged;



    private void Init()
    {
        //������ ������
        //inventoryItem = new Dictionary<int, ItemData>();
        slotItemDatas= new List<SlotItemData>();
        //���� ������ŭ ���������ϴ°�?
        for (int i = 0; i < MaxSlots; i++)
        {
            slotItemDatas.Add(new SlotItemData());
        }
    }

    public bool AddInventoryitme(ItemData itemData)
    {
        //slotItemDatas�� ������ �߰� 
        var emptySlot = slotItemDatas.Find(slot => slot.IsEmpty);
        if (emptySlot == null)
        {
            emptySlot.AddItem(itemData);
            OnInventoryChanged?.Invoke(); //���� �̺�Ʈ ����
            return true;
        }
        return false;
    }

    public bool RemoveInventoryitme(ItemData itemData)
    {
        //slotItemDatas���� ������ ���� 

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
