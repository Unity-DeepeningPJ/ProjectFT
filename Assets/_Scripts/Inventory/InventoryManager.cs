using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public int MaxSlots = 16;

    public List<SlotItemData> slotItemDatas;
    public event Action OnInventoryChanged;
    [SerializeField] private UiInventory inventoryUI;
    [SerializeField] private UIIventoryScrean uiinventoryScrean;
    [SerializeField] private StateBackGround statebackground;

    [SerializeField] private List<ItemData> BaseItemData;
    private void Awake()
    {

    }
    private void Start()
    {
        //var currency=GameManager.Instance.PlayerManager.player.Currency;
        //currency.GoldAdd(CurrenyType.Gold, 10);
        //currency.GoldAdd(CurrenyType.Gold, -20);

        Init();
        inventoryUI.Init();
        uiinventoryScrean.init();
        statebackground.Init();

        GameManager.Instance.PlayerManager.player.PlayerState.UpdateEquipStats(0, 0, 0, 0);
    }
    private void Init()
    {
        slotItemDatas = new List<SlotItemData>();

        for (int i = 0; i < MaxSlots; i++)
        {
            slotItemDatas.Add(new SlotItemData());
        }
        //기본 장착 아이템 추가 
        AddItemlist();
    }

    public bool AddInventoryitme(ItemData itemData)
    {

        var emptySlot = slotItemDatas.Find(slot => slot.IsEmpty);
        if (emptySlot != null)
        {
            emptySlot.AddItem(itemData);
            //slotItemDatas.Add(emptySlot);
            //OnInventoryChanged?.Invoke();
            return true;
        }
        return false;
    }

    public bool RemoveInventoryitme(ItemData itemData)
    {

        // item 이 null인데 find를 찾을려고 해서 버그
        // null 검사를 하고 id를 비교해야됨 
        var ExistItme = slotItemDatas.Find(slotitem =>
        slotitem != null &&
        slotitem.item != null &&
        slotitem.item.id == itemData.id);
        if (ExistItme != null)
        {
            ExistItme.RemoveItem(itemData);
            //아이템 장착을 해제 +E표시 제거 + 능력치 해제 


            //slotItemDatas.Remove(ExistItme);
            //OnInventoryChanged?.Invoke();
            return true;
        }
        return false;
    }

    // 아이템 개수 return 
    public int CountingSlotItemData()
    {
        int count = 0;
        for (int i = 0; i < slotItemDatas.Count; i++)
        {
            if (!slotItemDatas[i].IsEmpty)
            {
                count++;
            }
        }

        return count;
    }

    public void ArrayInventory()
    {
        // 전체 슬롯 복사  참조 복사 문제 
        //var slots = new List<SlotItemData>(slotItemDatas);

        // 아이템 정보만 복사 (깊은 복사)
        List<ItemData> validItems = new List<ItemData>();
        List<int> validAmounts = new List<int>();

        // 유효한 아이템 정보만 따로 저장
        foreach (var slot in slotItemDatas)
        {
            if (!slot.IsEmpty)
            {
                validItems.Add(slot.item);  // ItemData 참조 저장
                validAmounts.Add(slot.amount);
            }
        }


        // 모든 슬롯 초기화 
        for (int i = 0; i < slotItemDatas.Count; i++)
        {
            slotItemDatas[i].RemoveItem(null);
        }

        // 저장해둔 아이템을 앞에서부터 채우기
        for (int i = 0; i < validItems.Count; i++)
        {
            slotItemDatas[i].AddItem(validItems[i], validAmounts[i]);
        }

        OnInventoryChanged?.Invoke();
    }


    public void AddItemlist()
    {
        for (int i = 0; i < BaseItemData.Count; i++)
        {
            AddInventoryitme(BaseItemData[i]);
        }
        ArrayInventory();
    }

    public void Removeitem_test()
    {
        for (int i = 0; i < BaseItemData.Count; i++)
        {
            RemoveInventoryitme(BaseItemData[i]);
            GameManager.Instance.EquipManager.UnequipItem(BaseItemData[i]);
        }
        ArrayInventory();
    }


    public void Removeitem_one__test()
    {

        RemoveInventoryitme(BaseItemData[1]);

        ArrayInventory();
    }



    //public void InventoryItemAdd(ItemData item)
    //{
    //    inventoryItem.Add(item.id, item);
    //}
}
