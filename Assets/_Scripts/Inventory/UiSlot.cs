using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiSlot : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Button slotButton;

    private void Start()
    {
        slotButton.onClick.AddListener(onSlotClick);
    }

    private void onSlotClick()
    {
        // 슬롯 클릭 이벤트
    }

    public void UpdateSlot(SlotItemData slot)
    {
        if (!slot.IsEmpty)
        {
            //존재하면 이미지 update해주기 
            iconImage.sprite = slot.item.Icon;
            //iconImage.enabled = true;
        }
        
    }

}
