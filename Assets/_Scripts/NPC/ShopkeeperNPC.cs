using System.Collections.Generic;
using UnityEngine;

public class ShopkeeperNPC : BaseNPC
{
    [Header("상점 설정")]
    [SerializeField] private UIStore shopUI;
    
    private bool isShopOpen = false;
    public override void Interact(PlayerController player)
    {
        // 상점 UI 띄우기
        if (shopUI != null && !isShopOpen)
        {
            shopUI.OpenShop();
            isShopOpen = true;
        }
        else
        {
            shopUI.StoreExit();
            isShopOpen = false;
            Debug.LogError($"상점 NPC '{npcName}'에 ShopUI가 할당되지 않았거나 이미 상점이 열려있습니다.");
        }
    }
    // 
}