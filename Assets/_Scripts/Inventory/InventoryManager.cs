using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    //�κ��丮 ���� list 
    // �κ��丮 ���� ������list 
    // �κ��丮�� �������� �ٽ� �ʱ�ȭ ���ִ� �̺�Ʈ�� �����ߵ� 
    public int MaxSlots = 16;


    public Dictionary<int, ItemData> inventoryItem;

    public event Action OnInventoryChanged;



    private void Init()
    {
        //������ ������
        inventoryItem = new Dictionary<int, ItemData>();
        

        

    }
}
