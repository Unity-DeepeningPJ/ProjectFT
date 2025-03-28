using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CurrenyType
{
    Gold
}

public class CurrencyManager : MonoBehaviour
{
    public Dictionary<CurrenyType, int> currencies;

    public CurrencyManager()
    {
        currencies = new Dictionary<CurrenyType, int>();

        currencies[CurrenyType.Gold] = 0;
    }


    public void GoldAdd(CurrenyType currenyType, int amount)
    {
        if (currencies.TryGetValue(currenyType, out int currentglod))
        {
            currencies[currenyType] = currentglod + amount;
            Debug.Log($"현재 골드: {currencies[currenyType]}");
        }
        else
        {
            Debug.Log("골드가 없습니다.");
        }
    }
}
