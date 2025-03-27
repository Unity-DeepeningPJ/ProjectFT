using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIStore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI goldTxt;

    ItemDataList itemDataList;
    InventoryManager inven;

    private void Awake()
    {
        itemDataList = new ItemDataList();
        itemDataList.Init();
        inven = FindObjectOfType<InventoryManager>();        
    }

    private void UpdateGold()
    {
        // 플레이어 골드 가져와서 업데이트 로직
        //goldTxt.text = string.Format("{0:N0}, 플레이어 골드");
    }
    
}
