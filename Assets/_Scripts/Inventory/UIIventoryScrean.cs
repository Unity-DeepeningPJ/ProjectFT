using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIIventoryScrean : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI inventoryVlume;


    private void Start()
    {
        GameManager.Instance.InventoryManager.OnInventoryChanged += OnChangeSpace;
        init();
    }


    private void init()
    {
        OnChangeSpace();
    }

    private void OnChangeSpace()
    {
        // 공간 개수 변경시  TEXT 변경


        //max 공간으로 존재 : 안에 item 존재유무를 확인해서 counting해야된다.
        //관련 메소드를 만들어야겠지 ? inventorymanager에서 

        int ExistItemCount = GameManager.Instance.InventoryManager.CountingSlotItemData();


        inventoryVlume.text = $"Inventory : {ExistItemCount}/" +
                                            $"{GameManager.Instance.InventoryManager.slotItemDatas.Count}";

    }

}
