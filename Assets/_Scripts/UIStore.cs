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

        UpdateGold();

        buyUI.buy += (message) => ToggleComplete();
        buyUI.buy += CompleteTxt;

        sellUI.sell += ToggleComplete;
        sellUI.sell += () => CompleteTxt("판매 완료");
        
    }

    private void UpdateGold()
    {
        if (GameManager.Instance.PlayerManager.player.Currency.currencies.TryGetValue(CurrenyType.Gold, out int gold))
        {
            int curGold = gold;
            goldTxt.text = string.Format("{0:N0}", curGold);
        }        
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
            UpdateGold();
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

    public void StoreExit()
    {
        gameObject.SetActive(false);
    }

    public void OpenShop()
    {
        gameObject.SetActive(true);
    }
}
