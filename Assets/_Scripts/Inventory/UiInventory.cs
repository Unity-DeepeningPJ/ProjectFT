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

        GameManager.Instance.inventoryManager.OnInventoryChanged += UpdateUI;

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

    // ���Կ� itemdata �־��ֱ� 
    private void UpdateUI()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < GameManager.Instance.inventoryManager.slotItemDatas.Count)
            {
                slots[i].UpdateSlot(GameManager.Instance.inventoryManager.slotItemDatas[i]);
            }
        }
    }

}
