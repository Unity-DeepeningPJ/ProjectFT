using System.Collections.Generic;
using UnityEngine;

public class ShopkeeperNPC : BaseNPC
{
    [Header("상점 설정")]
    [SerializeField] private List<ShopItem> shopItems = new List<ShopItem>();
    [SerializeField] private UIStore shopUI;
    
    [System.Serializable]
    public class ShopItem
    {
        public string itemName;
        public Sprite icon;
        public int price;
        public string description;
        public ItemData itemPrefab;
    }
    
    public override void Interact(PlayerController player)
    {
        // 상점 UI 띄우기
        if (shopUI != null)
        {
            shopUI.OpenShop(this, player);
            GameManager.Instance.SetGameState(GameManager.GameState.Paused);
        }
        else
        {
            Debug.LogError($"상점 NPC '{npcName}'에 ShopUI가 할당되지 않았습니다!");
        }
    }
    
    public List<ShopItem> GetShopItems()
    {
        return shopItems;
    }
}