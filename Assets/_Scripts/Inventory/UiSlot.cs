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
        //비어있다면 Image를 없애줘야지 
        if (!slot.IsEmpty)
        {
            //존재하면 이미지 update해주기 
            iconImage.sprite = slot.item.Icon;
            //iconImage.enabled = true;
        }
        else
        {
            //기본 base이미지를 가줘와야하나?
            // null 이 될까?
            iconImage.sprite = null;
        }
        
    }

}
