using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipManager : MonoBehaviour
{
    private Dictionary<EquipType, ItemData> EqipDictionary;


    public EquipManager()
    {
        EqipDictionary = new Dictionary<EquipType, ItemData>();

        
    }
}
