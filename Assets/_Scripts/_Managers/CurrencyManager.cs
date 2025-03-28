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
        currencies.Add(currenyType, amount);
    }
}
