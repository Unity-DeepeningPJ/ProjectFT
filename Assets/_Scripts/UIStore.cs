using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI goldTxt;
    [SerializeField] GameObject completePopup;
    [SerializeField] Button completeBtn;
    [SerializeField] TextMeshProUGUI completeTxt;
    [SerializeField] Button exitBtn;

    UIItemSell sellUI;
    UIItemBuy buyUI;


    private void Awake()
    {
        sellUI = GetComponentInChildren<UIItemSell>();
        buyUI = GetComponentInChildren<UIItemBuy>();

        exitBtn.onClick.AddListener(StoreExit);
        completeBtn.onClick.AddListener(OnClickCompleteButton);
        buyUI.buy += ToggleComplete;
        buyUI.buy += () => CompleteTxt("구매 완료");

    }

    private void UpdateGold()
    {
        // 플레이어 골드 가져와서 업데이트 로직
        //goldTxt.text = string.Format("{0:N0}, 플레이어 골드");
    }
    
    private void ToggleComplete()
    {
        if (completePopup.activeInHierarchy)
        {
            completePopup.SetActive(false);
        }
        else
        {
            completePopup.SetActive(true);
        }
    }

    private void OnClickCompleteButton()
    {
        ToggleComplete();
    }

    private void CompleteTxt(string message)
    {
        completeTxt.text = message;
    }

    private void StoreExit()
    {
        gameObject.SetActive(false);
    }
}
