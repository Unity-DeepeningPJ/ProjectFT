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
        //플레이어 스텟 초기화 작업 >원래는 여기서 하면 안될거같은데 다른분 코드 건들면 안되니까 여기서 실행 
        GameManager.Instance.PlayerManager.player.PlayerState.UpdateEquipStats(0, 0, 0, 0);
    }

    public void EqipDictionaryAddItem(ItemData itemData)
    {
        //근데 같은 타입의 데이터가 있으면 안됨 따라서 검사를 하고 그 아이템을 해제하고 이 아이템을 넣어줘야됨 
        UnequipItem(itemData.type);

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
    public void UnequipItem(EquipType type)
    {
        if (EqipDictionary.ContainsKey(type))
        {
            ItemData unequippedItem = EqipDictionary[type];
            //기존 장비 해제 



            EqipDictionary.Remove(type);
            //Debug.Log("장착 아이템 삭제 완료 ");
            //해제시 이벤트 처리 
            // inventory에서 아이템 해제 이미지 보여줘야됨 
            // 능력치도 해제시켜줘야됨 
            AddStats();
            OnEquipmentRemoved?.Invoke(unequippedItem);


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
