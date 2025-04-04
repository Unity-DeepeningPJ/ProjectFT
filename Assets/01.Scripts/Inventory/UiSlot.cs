using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiSlot : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Button slotButton;
    [SerializeField] private TextMeshProUGUI text_equip;

    //슬롯별로 데이터를 가지고 있어야하네 

    public SlotItemData currentItemData;

    //아이템 클릭 이벤트(외부에서 구독 가능)
    public Action<SlotItemData> OnItemClicked;
    public Action<SlotItemData> OnItemDoubleClicked;

    // 더블 클릭 관련 변수
    private float lastClickTime = 0f;
    private float doubleClickTimeThreshold = 0.3f; // 더블 클릭 인식 간격 (초)
    private Coroutine clickCoroutine;

    private void Start()
    {
        slotButton.onClick.AddListener(onSlotClick);
        
        text_equip.gameObject.SetActive(false);
    }

    private void onSlotClick()
    {
        //빈슬롯 
        if (currentItemData == null || currentItemData.IsEmpty)
        {
            Debug.Log("빈 슬롯 클릭");
            return;
        }

        // 이전 클릭 코루틴이 있다면 중단 (더블클릭으로 판단)
        if (clickCoroutine != null)
        {
            StopCoroutine(clickCoroutine);
            clickCoroutine = null;

            // 더블 클릭 처리
            Debug.Log($"더블 클릭: {currentItemData.item.itemName}");
            OnItemDoubleClicked?.Invoke(currentItemData);
            UpdateEquip();
        }
        else
        {
            // 새로운 클릭 시작
            clickCoroutine = StartCoroutine(SingleClickDelay());
        }

    }

    private IEnumerator SingleClickDelay()
    {
        Debug.Log($"싱글클릭 코루틴시작 : {currentItemData.item.itemName}");
        // 더블클릭 대기 시간만큼 기다림
        yield return new WaitForSecondsRealtime(doubleClickTimeThreshold);

        // 시간이 지나면 싱글클릭으로 처리
        Debug.Log($"클릭: {currentItemData.item.itemName}");
        OnItemClicked?.Invoke(currentItemData);

        clickCoroutine = null;
    }

    public void UpdateSlot(SlotItemData slot)
    {
        //슬롯에 데이터를 저장
        currentItemData = slot;

        //비어있다면 Image를 없애줘야지 
        if (!slot.IsEmpty)
        {
            //존재하면 이미지 update해주기 
            iconImage.sprite = slot.item.Icon;
            iconImage.enabled = true;
            UpdateEquip();
        }
        else
        {
            //이미지 비활성화
            iconImage.sprite = null;
            text_equip.gameObject.SetActive(false);
            // 컴포넌트 자체를 비활성화 렌더링 비용 절약 가능
            iconImage.enabled = false;
        }


    }

    public void UpdateEquip()
    {
        // 현재 장착된 아이템인지 정확히 체크
        bool isEquipped = false;
        if (GameManager.Instance.EquipManager.EqipDictionary.TryGetValue(currentItemData.item.type, out ItemData equippedItem))
        {
            // ID로 비교
            isEquipped = (equippedItem.id == currentItemData.item.id);
            // 또는 reference로 비교
            // isEquipped = (equippedItem == slot.item);
        }
        text_equip.gameObject.SetActive(isEquipped);

    }

    public void UpdateRemoveEquip()
    {
        text_equip.gameObject.SetActive(false);
    }
    //remove 됬을때 본인의 E를 삭제 시켜주는 이벤트를 연결 해 줘야됨 

}
