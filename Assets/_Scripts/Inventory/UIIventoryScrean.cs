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
        // ���� ���� �����  TEXT ����


        //max �������� ���� : �ȿ� item ���������� Ȯ���ؼ� counting�ؾߵȴ�.
        //���� �޼ҵ带 �����߰��� ? inventorymanager���� 

        int ExistItemCount = GameManager.Instance.InventoryManager.CountingSlotItemData();


        inventoryVlume.text = $"Inventory : {ExistItemCount}/" +
                                            $"{GameManager.Instance.InventoryManager.slotItemDatas.Count}";

    }

}
