using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EquipManager : MonoBehaviour
{
    public Dictionary<EquipType, ItemData> EqipDictionary { get; private set; }
    public Action<ItemData> OnEquipmentRemoved;
    public Action<ItemData> OnEquipmentAdd;

    public EquipManager()
    {
        EqipDictionary = new Dictionary<EquipType, ItemData>();

    }

    private void Start()
    {

    }

    public void EqipDictionaryAddItem(ItemData itemData)
    {
        //근데 같은 타입의 데이터가 있으면 안됨 따라서 검사를 하고 그 아이템을 해제하고 이 아이템을 넣어줘야됨 
        UnequipItem(itemData);

        //새 장비 장착 
        EqipDictionary.Add(itemData.type, itemData);
        //Debug.Log("장착 아이템 더하기 완료 ");
        // 장비 장착 이벤트 발생 
        // 여기서 능력치를 더해주기 

        // 모든 장착 아이템의 스탯 합산
        AddStats();
        OnEquipmentAdd?.Invoke(itemData);
    }


    //
    public void UnequipItem(ItemData itemData)
    {
        if (EqipDictionary.ContainsKey(itemData.type))
        {
            ItemData unequippedItem = EqipDictionary[itemData.type];
            //기존 장비 해제 

            //id값을 비교해줘야됨 
            if (unequippedItem.id == itemData.id)
            {
                EqipDictionary.Remove(itemData.type);
                //능력치 해제 
                AddStats();
                //해제 이벤트 
                OnEquipmentRemoved?.Invoke(unequippedItem);
            }
        }


    }

    private void AddStats()
    {
        int totalPower = 0;
        int totalDefense = 0;
        int totalHealth = 0;
        float totalCritical = 0;

        foreach (var item in EqipDictionary.Values)
        {
            switch (item.equipCondition.type)
            {
                case ConditionType.Power:
                    totalPower += item.equipCondition.value;
                    break;
                case ConditionType.Defense:
                    totalDefense += item.equipCondition.value;
                    break;
                case ConditionType.Health:
                    totalHealth += item.equipCondition.value;
                    break;
                case ConditionType.Critical:
                    totalCritical += item.equipCondition.value;
                    break;
            }
        }
        GameManager.Instance.PlayerManager.player.PlayerState.UpdateEquipStats(totalPower, totalDefense, totalHealth, totalCritical);
    }





}
