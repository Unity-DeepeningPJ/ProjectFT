// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;

// public class healthBarUI : MonoBehaviour
// {
//     [SerializeField] private ConditionBar healthBar = new ConditionBar();
    
//     private PlayerStats playerStats;
    
//     private void Awake()
//     {
//         // UI 요소 연결
//         healthBar.fillImage = healthFillImage;
//         healthBar.valueText = healthText;
//         healthBar.barContainer = healthBarContainer;
//     }
    
//     public void SetPlayerStats(PlayerStats stats)
//     {
//         // 기존 이벤트 연결 해제
//         if (playerStats != null)
//         {
//             playerStats.OnHealthChanged -= UpdateHealthBar;
//             playerStats.OnMaxHealthChanged -= UpdateMaxHealth;
//         }
        
//         playerStats = stats;
        
//         // 새 이벤트 연결
//         if (playerStats != null)
//         {
//             playerStats.OnHealthChanged += UpdateHealthBar;
//             playerStats.OnMaxHealthChanged += UpdateMaxHealth;
            
//             // 초기 세팅
//             healthBar.Initialize(playerStats.MaxHealth);
//             UpdateHealthBar(playerStats.CurrentHealth, playerStats.MaxHealth);
//         }
//     }
    
//     private void Update()
//     {
//         // 부드러운 애니메이션 업데이트
//         healthBar.UpdateUI();
//     }
    
//     // 체력 바 업데이트
//     private void UpdateHealthBar(float current, float max)
//     {
//         healthBar.SetValue(current, max);
//     }
    
//     // 최대 체력 변경 시 처리
//     private void UpdateMaxHealth(float newMaxHealth)
//     {
//         healthBar.UpdateBarSize(newMaxHealth);
//     }
    
//     private void OnDestroy()
//     {
//         // 이벤트 연결 해제
//         if (playerStats != null)
//         {
//             playerStats.OnHealthChanged -= UpdateHealthBar;
//             playerStats.OnMaxHealthChanged -= UpdateMaxHealth;
//         }
//     }
// }