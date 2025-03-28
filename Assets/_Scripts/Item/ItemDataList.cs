using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataList
{
    public Dictionary<int, ItemData> ItemList { get; private set; }

    public void Init()
    {
        ItemList = new Dictionary<int, ItemData>();
        LoadItemData();
        Debug.Log($"{ItemList.Count} 수량 아이템 업로드 했습니다.");
    }

    private void LoadItemData()
    {
        ItemData[] loadItems = Resources.LoadAll<ItemData>("ItemData");

        foreach (var item in loadItems)
        {
            ItemList.Add(item.id, item);
        }
    }

    public ItemData GetItemDataById(int id)
    {
        if (ItemList.TryGetValue(id, out ItemData itemData))
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