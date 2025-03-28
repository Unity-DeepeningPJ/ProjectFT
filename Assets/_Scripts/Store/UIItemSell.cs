using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemSell : UIBaseTrade
{
    List<UIStoreSlot> slots;

    InventoryManager inven;
    ItemData selecetItem;
    UIStoreSlot selectSlot;

    public Action sell;

    private void Awake()
    {

        tradeBtn.onClick.AddListener(OnClickSellItem);

        //아이템 수량만큼 슬롯 가져오기

        //for (int i = 0; i < inven.inventoryItem.Count; i++)
        //{
        //    Instantiate(slotPrefap, slotsTransform);
        //    slots[i] = slotsTransform.GetChild(i).GetComponent<UIStoreSlot>();
        //    slots[i].index = i;
        //}

        //UpateSellUI();
    }

    private void Start()
    {
        inven = GameManager.Instance.InventoryManager;
    }
    private void UpateSellUI()
    {
        //for (int i = 0; i < inven.inventoryItem.Count; i++)
        //{
        //    if (inven.inventoryItem.TryGetValue(i, out ItemData item))
        //    {
        //        slots[i].item = item;
        //        slots[i].SetSlot(item);
        //    }
        //}
    }

    //public override void SelectSlot(int index)
    //{
    //    if (selectSlot != null)
    //    {
    //        selectSlot.GetComponent<Outline>().enabled = false;
    //    }

    //    selecetItem = slots[index].item;
    //    selectSlot = slots[index];
    //    selectSlot.GetComponent<Outline>().enabled = true;
    //}

    public void OnClickSellItem()
    {
        if (selecetItem == null) return;

        //sell?.Invoke();

        Debug.Log($"{selecetItem.gold} 골드 증가");
        // selecetItem.gold 만큼 gold 관리하는 곳에서 증가

        selectSlot.GetComponent<Outline>().enabled = false;
        selecetItem = null;
        selectSlot = null;
    }
}
