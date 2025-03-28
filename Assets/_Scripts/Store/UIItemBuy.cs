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
    
    public Action buy;

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

        inven.AddInventoryitme(selecetItem);
        GameManager.Instance.PlayerManager.player.Currency.GoldAdd(CurrenyType.Gold, -selecetItem.gold);
        buy?.Invoke();        
        Debug.Log($"{selecetItem.gold} 골드 차감");
        // selecetItem.gold 만큼 gold 관리하는 곳에서 차감

        ClearPrevSelect();
    }

    private void ClearPrevSelect()
    {
        selectSlot.GetComponent<Outline>().enabled = false;
        selecetItem = null;
        selectSlot = null;
    }
}
