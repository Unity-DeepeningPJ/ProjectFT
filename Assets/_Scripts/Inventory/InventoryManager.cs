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

    //�׽�Ʈ�� ������ list
    [SerializeField] private List<ItemData> TestItemData;
    private void Start()
    {
        Init();
    }
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
        var emptySlot = slotItemDatas.Find(slot => slot.IsEmpty ==true);
        if (emptySlot == null)
        {
            emptySlot.AddItem(itemData);
            //slotItemDatas.Add(emptySlot);
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
            //slotItemDatas.Remove(ExistItme);
            OnInventoryChanged?.Invoke();
            return true;
        }
        return false;
    }


    //�׽�Ʈ �� 
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
