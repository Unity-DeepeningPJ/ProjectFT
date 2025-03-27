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
        GameManager.Instance.InventoryManager.OnInventoryChanged += UpdateUI;
        Init();
    }
    private void Init()
    {
        //���� ������  > UI�� �׷��ֱ� 
        for (int i = 0; i < GameManager.Instance.InventoryManager.MaxSlots; i++)
        {
            UiSlot slotobj = Instantiate(uiSlotPrefab, slotsparent);
            slots.Add(slotobj);
        }

        // �������ֱ�? 
    }

    // ���Կ� itemdata �־��ֱ� 
    private void UpdateUI()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < GameManager.Instance.InventoryManager.slotItemDatas.Count)
            {
                slots[i].UpdateSlot(GameManager.Instance.InventoryManager.slotItemDatas[i]);
            }
        }
    }

}
