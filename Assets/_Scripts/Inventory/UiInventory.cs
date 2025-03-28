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
        //슬롯 데이터 
        slots = new List<UiSlot>();

        
    }

    private void Start()
    {
        GameManager.Instance.InventoryManager.OnInventoryChanged += UpdateUI;
        
        Init();

        foreach (var slot in slots)
        {
            slot.OnItemClicked += HandleItemOneClicked;
            slot.OnItemDoubleClicked += HandleItemDubleClickd;
        }
    }

    private void HandleItemOneClicked(SlotItemData slotItemData)
    {
        Debug.Log($"원클릭 :{slotItemData.item.itemName}");



    }
    private void HandleItemDubleClickd(SlotItemData slotItemData)
    {
        //더블클릭 이벤트처리 
        Debug.Log($"더블클릭 :{slotItemData.item.itemName}");
    }

    private void Init()
    {
        //슬롯 생성만  > UI도 그려주기 
        for (int i = 0; i < GameManager.Instance.InventoryManager.MaxSlots; i++)
        {
            UiSlot slotobj = Instantiate(uiSlotPrefab, slotsparent);
            slots.Add(slotobj);
        }

        // 생성해주기? 
    }

    // 슬롯에 itemdata 넣어주기 
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
