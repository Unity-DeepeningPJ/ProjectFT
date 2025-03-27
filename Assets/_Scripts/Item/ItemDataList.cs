using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataList
{
    public Dictionary<int, ItemData> itemList;

    public void Init()
    {
        LoadItemData();
        Debug.Log($"{itemList.Count} 아이템 업로드 했습니다.");
    }

    private void LoadItemData()
    {
        ItemData[] loadItems = Resources.LoadAll<ItemData>("ItemData");

        foreach (var item in loadItems)
        {
            itemList.Add(item.id, item);
        }
    }

    public ItemData GetItemDataById(int id)
    {
        if (itemList.TryGetValue(id, out ItemData itemData))
        {
            return itemData;
        }
        else
        {
            Debug.Log($"{id} 에 해당하는 데이터가 없습니다.");
            return null;
        }
    }
}