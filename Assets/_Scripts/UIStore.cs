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
        // �÷��̾� ��� �����ͼ� ������Ʈ ����
        //goldTxt.text = string.Format("{0:N0}, �÷��̾� ���");
    }
    
}
