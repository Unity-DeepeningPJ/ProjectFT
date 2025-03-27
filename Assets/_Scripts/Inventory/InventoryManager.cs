using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    //인벤토리 슬롯 list 
    // 인벤토리 보유 아이템list 
    // 인벤토리가 변했을때 다시 초기화 해주는 이벤트도 만들어야됨 
    public int MaxSlots = 16;


    public Dictionary<int, ItemData> inventoryItem;

    public event Action OnInventoryChanged;



    private void Init()
    {
        //아이템 데이터
        inventoryItem = new Dictionary<int, ItemData>();
        

        

    }
}
