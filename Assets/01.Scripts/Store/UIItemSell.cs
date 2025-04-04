using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemSell : UIBaseTrade
{
    UIStoreSlot[] slots;
    InventoryManager inven;
    public ItemData selecetItem;
    public UIStoreSlot selectSlot;

    public Action sell;

    private void Awake()
    {
        inven = GameManager.Instance.InventoryManager;
        tradeBtn.onClick.AddListener(OnClickSellItem);

        slots = new UIStoreSlot[inven.MaxSlots];

        for (int i = 0; i < inven.MaxSlots; i++)
        {
            Instantiate(slotPrefap, slotsTransform);
            slots[i] = slotsTransform.GetChild(i).GetComponent<UIStoreSlot>();
            slots[i].index = i;
        }
    }

    private void OnEnable()
    {
        UpateSellUI();
    }

    public void UpateSellUI()
    {
        List<SlotItemData> datas = inven.slotItemDatas;
        tradeBtn.onClick.AddListener(ClearPrevSelect);

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < datas.Count && !datas[i].IsEmpty)
            {
                slots[i].SetSlot(datas[i].item);
                slots[i].gameObject.SetActive(true);
            }
            else
            {
                slots[i].ClearSlot();
                slots[i].gameObject.SetActive(false);
            }
        }
    }

    public override void SelectSlot(int index)
    {
        if (selectSlot != null)
        {
            ClearPrevSelect();
        }

        selecetItem = slots[index].item;
        selectSlot = slots[index];
        selectSlot.GetComponent<Outline>().enabled = true;
    }

    public void OnClickSellItem()
    {
        if (selecetItem == null) return;

        if (selecetItem.type != EquipType.Consumealbe)
        {
            inven.RemoveInventoryitme(selecetItem);
            GameManager.Instance.EquipManager.UnequipItem(selecetItem);
            inven.ArrayInventory();
            BuyTradeComplete(); return;
        }
        
        var data = inven.slotItemDatas.Find(data => data.item.type == EquipType.Consumealbe);
        if (data.amount == 1)
        {
            inven.RemoveInventoryitme(selecetItem);
            inven.ArrayInventory();
            BuyTradeComplete();
        }
        else
        {
            data.amount--;
            BuyTradeComplete();
        }
    }

    private void ClearPrevSelect()
    {
        if (selectSlot == null) return;
        
        selectSlot.GetComponent<Outline>().enabled = false;
        selecetItem = null;
        selectSlot = null;
    }

    private void BuyTradeComplete()
    {
        GameManager.Instance.PlayerManager.player.Currency.GoldAdd(CurrenyType.Gold, selecetItem.gold);
        sell?.Invoke();
        ClearPrevSelect();
        UpateSellUI();        
    }
}
