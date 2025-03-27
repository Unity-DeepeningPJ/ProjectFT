using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiInventory : MonoBehaviour
{
    
    public List<UiSlot> slots;
    [SerializeField] private UiSlot uiSlotPrefab;
    [SerializeField] private Transform slotsparent;


    private void Awake()
    {
        //���� ������ 
        slots = new List<UiSlot>();



    }

    private void Start()
    {
        Init();
    }
    private void Init()
    {
        

        //���� ������  > UI�� �׷��ֱ� 
        for (int i = 0; i < GameManager.Instance.inventoryManager.MaxSlots; i++)
        {
            UiSlot slotobj =Instantiate(uiSlotPrefab, slotsparent);
            slots.Add(slotobj);
        }

        // �������ֱ�? 
    }

    // ������ �����ִ� ��� 



}
