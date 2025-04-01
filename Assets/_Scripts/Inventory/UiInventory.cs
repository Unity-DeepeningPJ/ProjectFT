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


    }

    private void Start()
    {
        

        //Init();


    }

    private void HandleItemOneClicked(SlotItemData slotItemData)
    {
        //Debug.Log($"원클릭 :{slotItemData.item.itemName}");



    }
    private void HandleItemDubleClickd(SlotItemData slotItemData)
    {
        //더블클릭 이벤트처리 
        //Debug.Log($"더블클릭 :{slotItemData.item.itemName}");

        // 그 부위에 아이템이 장착 되어있나 없나 
        if (GameManager.Instance.EquipManager.EqipDictionary.TryGetValue(slotItemData.item.type, out ItemData item))
        {
            // 그게 장착된 아이템인가? 아닌가 ?
            if (item.id == slotItemData.item.id)
            {
                //장착되어있다면 >해제 
                GameManager.Instance.EquipManager.UnequipItem(item);
            }
            else // 미리 장착된 아이템이 있고 지금 장착할 아이템은 다른 아이템 
            {
                //해제 전에 기존 장비의 E를 없애 줘야됨 
                var oldItem = slots.Find(slot => slot.currentItemData.item.id == item.id);
                if (oldItem != null)
                {
                    oldItem.UpdateRemoveEquip();
                }
                // 그게 아니라면 > 보유아이템 해제 > 슬롯아이템 더해줘
                GameManager.Instance.EquipManager.UnequipItem(item);
                GameManager.Instance.EquipManager.EqipDictionaryAddItem(slotItemData.item);
            }
        }
        else
        {
            //비장착 아이템 > 장착 
            GameManager.Instance.EquipManager.EqipDictionaryAddItem(slotItemData.item);
        }
    }

    public void Init()
    {
        GameManager.Instance.InventoryManager.OnInventoryChanged += UpdateUI;

        //슬롯 데이터 
        slots = new List<UiSlot>();
        //슬롯 생성만  > UI도 그려주기 
        for (int i = 0; i < GameManager.Instance.InventoryManager.MaxSlots; i++)
        {
            UiSlot slotobj = Instantiate(uiSlotPrefab, slotsparent);
            slots.Add(slotobj);
        }

        UpdateUI();
        // 생성해주기? 

        foreach (var slot in slots)
        {
            slot.OnItemClicked += HandleItemOneClicked;
            slot.OnItemDoubleClicked += HandleItemDubleClickd;
        }

    }

    // 슬롯에 itemdata 넣어주기  + Equip 표시 넣어주기 (장착한 아이템만)
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
