using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemBuy : UIBaseTrade
{
    UIStoreSlot[] slots;

    ItemDataList itemDataList;
    ItemData selecetItem;
    UIStoreSlot selectSlot;

    public Action buy;

    private void Awake()
    {
        itemDataList = new ItemDataList();
        itemDataList.Init();
        tradeBtn.onClick.AddListener(OnClickBuyItem);

        //������ ������ŭ ���� ��������
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

        
        buy?.Invoke();

        Debug.Log($"{selecetItem.gold} ��� ����");
        // selecetItem.gold ��ŭ gold �����ϴ� ������ ����

        ClearPrevSelect();
    }

    private void ClearPrevSelect()
    {
        selectSlot.GetComponent<Outline>().enabled = false;
        selecetItem = null;
        selectSlot = null;
    }
}
