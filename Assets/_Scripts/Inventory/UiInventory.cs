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
        
        // 그 부위에 아이템이 장착 되어있나 없나 
        if (GameManager.Instance.EquipManager.EqipDictionary.TryGetValue(slotItemData.item.type, out ItemData item))
        {
            // 그게 장착된 아이템인가? 아닌가 ?
            if (item.id == slotItemData.item.id)
            {
                //장착되어있다면 >해제 
                GameManager.Instance.EquipManager.UnequipItem(item.type);
            }
            else
            {
                // 그게 아니라면 > 보유아이템 해제 > 슬롯아이템 더해줘
                GameManager.Instance.EquipManager.UnequipItem(item.type);
                GameManager.Instance.EquipManager.EqipDictionaryAddItem(slotItemData.item);
            }
        }
        else
        {
            //비장착 아이템 > 장착 
            GameManager.Instance.EquipManager.EqipDictionaryAddItem(slotItemData.item);
        }
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
