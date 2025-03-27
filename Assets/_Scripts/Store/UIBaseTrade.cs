using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBaseTrade : MonoBehaviour
{
    //[SerializeField] Button tradeBtn;

    [SerializeField] protected Transform slotsTransform;
    [SerializeField] protected GameObject slotPrefap;
    [SerializeField] protected Button tradeBtn;

    private void Awake()
    {
            
    }

    public virtual void SelectSlot(int index)
    {
        
    }

}
