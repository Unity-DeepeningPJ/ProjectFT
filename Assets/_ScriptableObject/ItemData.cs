using System;
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
public enum ConditionType
{
    Power,
    Defense,
    Health,
    Critical
}

[Serializable]
public class EquipCondition
{
    public ConditionType type;
    public int value;
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public int id;
    public EquipType type;
    public string itemName;
    public string description;

    public EquipCondition equipCondition;

    public int gold;


    public Sprite Icon;
    public GameObject itemObj;
}
