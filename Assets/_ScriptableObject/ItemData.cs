using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public enum EquipType
{
    Weapon,
    Coat,
    Shield,
    Glove
}

[SerializeField]
public class EquipItem
{
    public EquipType equipType;
    public float value;
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

    public Sprite Icon;
    public GameObject itemObj;
}
