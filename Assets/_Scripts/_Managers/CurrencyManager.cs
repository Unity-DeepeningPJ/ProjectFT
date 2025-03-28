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


    public bool GoldAdd(CurrenyType currenyType, int amount)
    {
        if (currencies.TryGetValue(currenyType, out int currentglod))
        {
            int addglod = currentglod + amount;
            if (addglod < 0)
            {
                Debug.Log("골드가 부족합니다.");
                return false;
            }
            else
            {
                currencies[currenyType] = addglod;
                Debug.Log($"현재 골드: {currencies[currenyType]}");
                return true;
            }
        }
        else
        {
            Debug.Log("골드가 없습니다.");
            return false;
        }
    }
}
