using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIStore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI goldTxt;


    private void Awake()
    {
            
    }

    private void UpdateGold()
    {
        // 플레이어 골드 가져와서 업데이트 로직
        //goldTxt.text = string.Format("{0:N0}, 플레이어 골드");
    }
    
}
