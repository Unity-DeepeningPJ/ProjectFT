using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIIventoryScrean : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI inventoryVlume;
    [SerializeField] private Image Slot_Equip_Coat;
    [SerializeField] private Image Slot_Equip_Glove;
    [SerializeField] private Image Slot_Equip_Shield;
    [SerializeField] private Image Slot_Equip_Weapon;
    [SerializeField] private TextMeshProUGUI Name;
    [SerializeField] private TextMeshProUGUI Gold;
    [SerializeField] private SpriteRenderer coatSprite;
    [SerializeField] private SpriteRenderer weaponSprite;
    [SerializeField] private Image Posion_Inventory;
    [SerializeField] private Image Posion_MainUI;


    private void Start()
    {
;

        //init();
    }
    public void init()
    {
        //이벤트 연결
        GameManager.Instance.InventoryManager.OnInventoryChanged += OnChangeSpace;
        GameManager.Instance.EquipManager.OnEquipmentAdd += OnChangeAddEquipItems;
        GameManager.Instance.EquipManager.OnEquipmentRemoved += OnChangeRemoveEquipItems;
        GameManager.Instance.PlayerManager.player.Currency.OnGoldChange += OnGoldChangeText;
        //골드 Currency 초기화 ?? 어디서 해야할지 모르겟음 
        GameManager.Instance.PlayerManager.player.Currency.init_OngoldChange();
        OnChangeSpace();
        ChangeName_text();
    }

    private void ChangeName_text()
    {
        Name.text = GameManager.Instance.PlayerManager.player.PlayerState.Name;
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

    private void OnChangeAddEquipItems(ItemData itemData)
    {
        if (GameManager.Instance.EquipManager.EqipDictionary.TryGetValue(itemData.type, out ItemData item))
        {
            // 보유아이템
            switch (item.type)
            {
                case EquipType.Consumealbe:
                    Posion_Inventory.sprite = item.Icon;
                    Posion_MainUI.sprite =item.Icon;
                    break;
                case EquipType.Weapon:
                    Slot_Equip_Weapon.sprite = item.Icon;
                    weaponSprite.sprite = item.Icon;
                    break;
                case EquipType.Coat:
                    Slot_Equip_Coat.sprite = item.Icon;
                    coatSprite.sprite = item.Icon;
                    break;
                case EquipType.Shield:
                    Slot_Equip_Shield.sprite = item.Icon;
                    break;
                case EquipType.Glove:
                    Slot_Equip_Glove.sprite = item.Icon;
                    break;
                default:
                    break;
            }
        }

    }

    private void OnChangeRemoveEquipItems(ItemData itemData)
    {
        if (!GameManager.Instance.EquipManager.EqipDictionary.TryGetValue(itemData.type, out ItemData item))
        {
            //보유하지 않은 아이템 
            // 보유아이템
            switch (itemData.type)
            {
                case EquipType.Consumealbe:
                    Posion_Inventory.sprite =null;
                    Posion_MainUI.sprite = null;
                    break;
                case EquipType.Weapon:
                    Slot_Equip_Weapon.sprite = null;
                    weaponSprite.sprite = null;
                    break;
                case EquipType.Coat:
                    Slot_Equip_Coat.sprite = null;
                    coatSprite.sprite = null;
                    break;
                case EquipType.Shield:
                    Slot_Equip_Shield.sprite = null;
                    break;
                case EquipType.Glove:
                    Slot_Equip_Glove.sprite = null;
                    break;
                default:
                    break;
            }
        }
    }
    private void OnGoldChangeText(int gold)
    {
        Gold.text = gold.ToString();
    }
}
