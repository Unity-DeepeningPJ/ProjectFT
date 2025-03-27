using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equipment,
    consumable 
}

public enum EquipType
{
    Weapon,
    Coat,
    Shield,
    Glove
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public ItemType type;
    public string itemName;
    public string description;
    public Sprite Icon;
    public GameObject itemObj;
    
}
