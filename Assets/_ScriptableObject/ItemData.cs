using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public enum EquipType
{
    Consumealbe,
    Weapon,
    Coat,
    Shield,
    Glove
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public int id;
    public EquipType type;
    public string itemName;
    public string description;
    public int value;
    public int gold;


    public Sprite Icon;
    public GameObject itemObj;
}
