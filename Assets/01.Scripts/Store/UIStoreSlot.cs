using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIStoreSlot : MonoBehaviour
{
    [SerializeField] Button selcetBtn;
    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI nameTxt;
    [SerializeField] TextMeshProUGUI descripTxt;
    [SerializeField] TextMeshProUGUI typeTxt;
    [SerializeField] TextMeshProUGUI goldTxt;
    [SerializeField] TextMeshProUGUI amountTxt;

    public int index;

    public ItemData item;

    private void Start()
    {
        selcetBtn.onClick.AddListener(OnClinkSelctSlot);
    }

    public void SetSlot(ItemData item)
    {
        this.item = item;
        itemImage.sprite = item.Icon;
        nameTxt.text = item.itemName.ToString();
        descripTxt.text = item.description.ToString();
        goldTxt.text = item.gold.ToString();
        typeTxt.text = $"{item.equipCondition.type} + {item.equipCondition.value}";
    }

    public void ClearSlot()
    {
        item = null;
        itemImage.sprite = null;
        nameTxt.text = "";
        descripTxt.text = "";
        typeTxt.text = "";
    }

    public void OnClinkSelctSlot()
    {
        var currentTrade = GetComponentInParent<UIBaseTrade>();

        if (currentTrade != null)
        {
            currentTrade.SelectSlot(index);
        }
    }
}
