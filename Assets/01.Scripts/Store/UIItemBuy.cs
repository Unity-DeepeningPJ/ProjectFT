using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemBuy : UIBaseTrade
{
    UIStoreSlot[] slots;
    InventoryManager inven;
    ItemDataList itemDataList;
    ItemData selecetItem;
    UIStoreSlot selectSlot;

    UIItemSell uIItemSell;

    public Action<string> buy;

    private void Awake()
    {
        inven = GameManager.Instance.InventoryManager;
        itemDataList = new ItemDataList();
        itemDataList.Init();
        tradeBtn.onClick.AddListener(OnClickBuyItem);

        //아이템 수량만큼 슬롯 가져오기
        slots = new UIStoreSlot[itemDataList.ItemList.Count];

        for (int i = 0; i < itemDataList.ItemList.Count; i++)
        {
            Instantiate(slotPrefap, slotsTransform);
            slots[i] = slotsTransform.GetChild(i).GetComponent<UIStoreSlot>();
            slots[i].index = i;
        }

        uIItemSell = FindAnyObjectByType<UIItemSell>();

        UpateBuyUI();
    }

    private void UpateBuyUI()
    {
        for (int i = 0; i < itemDataList.ItemList.Count; i++)
        {
            if (itemDataList.ItemList.TryGetValue(i, out ItemData item))
            {
                slots[i].item = item;
                slots[i].SetSlot(item);
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

    public void OnClickBuyItem()
    {
        if (selecetItem == null) return;

        int gold = GameManager.Instance.PlayerManager.player.Currency.currencies[CurrenyType.Gold];

        if (selecetItem.gold > gold)
        {
            buy.Invoke("골드 부족");
            ClearPrevSelect();
            return;
        }

        if (selecetItem.type != EquipType.Consumealbe)
        {            
            //Debug.Log("장비 구입");
            inven.AddInventoryitme(selecetItem);
            inven.ArrayInventory();
            BuyTradeComplete();
        }
        else
        {
            for (int i = 0; i < inven.slotItemDatas.Count; i++)
            {
                if (inven.slotItemDatas[i].IsEmpty)
                {
                    inven.AddInventoryitme(selecetItem);
                    inven.ArrayInventory();
                    BuyTradeComplete();
                    return;
                }
                else if (inven.slotItemDatas[i].item.type == EquipType.Consumealbe)
                {
                    inven.slotItemDatas[i].AddItem(selecetItem);
                    BuyTradeComplete();
                    //Debug.Log(inven.slotItemDatas[i].amount);
                    return;                    
                }
                else if (inven.slotItemDatas[i].item.type != EquipType.Consumealbe)
                {
                    continue;
                }
            }
        }
    }

    private void ClearPrevSelect()
    {
        selectSlot.GetComponent<Outline>().enabled = false;
        selecetItem = null;
        selectSlot = null;
    }

    private void BuyTradeComplete()
    {
        GameManager.Instance.PlayerManager.player.Currency.GoldAdd(CurrenyType.Gold, -selecetItem.gold);
        buy?.Invoke("구매 완료");

        ClearPrevSelect();
        uIItemSell.UpateSellUI();
    }
}
