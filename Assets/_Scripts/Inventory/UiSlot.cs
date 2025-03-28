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
        // ���� Ŭ�� �̺�Ʈ
    }

    public void UpdateSlot(SlotItemData slot)
    {
        //����ִٸ� Image�� ��������� 
        if (!slot.IsEmpty)
        {
            //�����ϸ� �̹��� update���ֱ� 
            iconImage.sprite = slot.item.Icon;
            //iconImage.enabled = true;
        }
        else
        {
            //�⺻ base�̹����� ����;��ϳ�?
            // null �� �ɱ�?
            iconImage.sprite = null;
        }
        
    }

}
