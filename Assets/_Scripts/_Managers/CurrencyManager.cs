using System;
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
    public event Action<int> OnGoldChange;

    public CurrencyManager()
    {
        currencies = new Dictionary<CurrenyType, int>();

        //초기화 
        currencies[CurrenyType.Gold] = 0;
        
    }

    //골드를 set해주는 메소드 +이벤트 
    

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
                //여기서 골드 변환 이벤트 발생 
                AudioManager.Instance.PlaySFX("Coin");
                OnGoldChange?.Invoke(currencies[currenyType]);
                return true;
            }
        }
        else
        {
            Debug.Log("골드가 없습니다.");
            return false;
        }
    }

    public void init_OngoldChange()
    {
        OnGoldChange?.Invoke(0);
    }
}
