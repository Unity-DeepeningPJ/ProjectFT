using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemBuy : UIBaseTrade
{
    [SerializeField] Transform slotsTransform;
    [SerializeField] GameObject slotPrefap;
    [SerializeField] Button sellBtn;

    UIStoreSlot[] slots;

    ItemDataList itemDataList;
    ItemData selecetItem;
    UIStoreSlot selectSlot;

    private void Awake()
    {
        itemDataList = new ItemDataList();
        itemDataList.Init();
        sellBtn.onClick.AddListener(OnClickSellItem);

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

    public override void SelctSlot(int index)
    {
        selecetItem = slots[index].item;
        selectSlot = slots[index];
    }

    public void OnClickSellItem()
    {
        // selecetItem.gold 만큼 gold 관리하는곳에 넘겨주기
        Debug.Log($"{selecetItem.gold} 골드 주기");
    }
}
