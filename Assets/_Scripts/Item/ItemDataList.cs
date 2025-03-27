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
        Debug.Log($"{ItemList.Count} ���� ������ ���ε� �߽��ϴ�.");
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
            Debug.Log($"{id} �� �ش��ϴ� �����Ͱ� �����ϴ�.");
            return null;
        }
    }
}