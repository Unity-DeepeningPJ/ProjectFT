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

    public override void SelctSlot(int index)
    {
        selecetItem = slots[index].item;
        selectSlot = slots[index];
    }

    public void OnClickSellItem()
    {
        // selecetItem.gold ��ŭ gold �����ϴ°��� �Ѱ��ֱ�
        Debug.Log($"{selecetItem.gold} ��� �ֱ�");
    }
}
