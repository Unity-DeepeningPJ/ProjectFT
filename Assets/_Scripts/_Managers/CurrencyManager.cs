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


    public void GoldAdd(CurrenyType currenyType,int amount)
    {
        currencies.Add(currenyType, amount);
    }
}
