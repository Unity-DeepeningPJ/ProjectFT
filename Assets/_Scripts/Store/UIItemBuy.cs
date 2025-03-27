using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemBuy : UIBaseTrade
{
    ItemDataList itemDataList;

    private void Awake()
    {
        itemDataList = new ItemDataList();
        itemDataList.Init();
    }
}
