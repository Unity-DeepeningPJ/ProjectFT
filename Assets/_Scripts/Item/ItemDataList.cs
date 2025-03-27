using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataList
{
    public Dictionary<int, ItemData> itemList;

    public void Init()
    {
        LoadItemData();
        Debug.Log($"{itemList.Count} ������ ���ε� �߽��ϴ�.");
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
            Debug.Log($"{id} �� �ش��ϴ� �����Ͱ� �����ϴ�.");
            return null;
        }
    }
}