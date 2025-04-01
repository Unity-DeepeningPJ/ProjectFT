// PlayerUI.cs - 플레이어 UI 전체를 관리하는 메인 클래스
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [Header("체력 관련")]
    [SerializeField] private HealthBar healthBar;
    
    [Header("레벨과 경험치")]
    [SerializeField] private ExpBar experienceBar;
    [SerializeField] private TextMeshProUGUI levelText;
    
    [Header("골드")]
    [SerializeField] private TextMeshProUGUI goldText;
    
    // 플레이어 참조
    private Player player;
    private PlayerState playerState;
    private PlayerCondition playerCondition;
    private CurrencyManager playerGold;
    
    private void Start()
    {
        // 플레이어 찾기
        player = FindObjectOfType<Player>();
        if (player == null)
        {
            Debug.LogError("PlayerUI: 플레이어를 찾을 수 없습니다!");
            return;
        }
        
        // 필요한 컴포넌트 참조 가져오기
        playerState = player.GetComponent<PlayerState>();
        playerCondition = player.GetComponent<PlayerCondition>();
        playerGold = player.GetComponent<CurrencyManager>();
        
        // 초기 UI 세팅
        SetupUI();
        
        // 이벤트 구독
        SubscribeToEvents();
    }
    
    private void SetupUI()
    {
        if (playerState != null)
        {
            // 경험치바 초기화
            if (experienceBar != null)
            {
                Debug.Log($"경험치바 초기화: {playerState.CurrentExp}/{playerState.ExpToNextLevel}");
                experienceBar.SetValue(playerState.CurrentExp, playerState.ExpToNextLevel);
            }
            else
            {
                Debug.LogError("PlayerUI: experienceBar 참조가 null입니다!");
            }
            
            // 레벨 텍스트 초기화
            if (levelText != null)
                levelText.text = $"Lv. {playerState.Level}";
        }
        else
        {
            Debug.LogError("PlayerUI: playerState가 null입니다!");
        }
        
        // 골드 텍스트 초기화
        if (playerGold != null && goldText != null)
            goldText.text = $"Gold: {playerGold.currencies[CurrenyType.Gold]}";
    }
    
    private void SubscribeToEvents()
    {
        if (playerState != null)
        {
            // 레벨 변화 이벤트 구독
            playerState.OnLevelChanged += UpdateLevel;
            
            // 경험치 변화 이벤트 구독
            playerState.OnExpChanged += UpdateExperience;
        }
        
        if (playerGold != null)
        {
            // 골드 변화 이벤트 구독
            playerGold.OnGoldChange += UpdateGold;
        }
    }
    
    // 체력바 업데이트
    private void UpdateHealthBar(float current, float max)
    {
        if (healthBar != null)
            healthBar.SetValue(current, max);
    }
    
    // 최대 체력 업데이트
    private void UpdateMaxHealth(float newMaxHealth)
    {
        if (healthBar != null)
            healthBar.UpdateMaxValue(newMaxHealth);
    }
    
    // 레벨 업데이트
    private void UpdateLevel(int newLevel)
    {
        if (levelText != null)
            levelText.text = $"Lv. {newLevel}";
    }
    
    // 경험치 업데이트
    private void UpdateExperience(int current, int max)
    {
        Debug.Log($"경험치 업데이트 이벤트: {current}/{max}");
        
        if (experienceBar != null)
            experienceBar.SetValue(current, max);
        else
            Debug.LogError("PlayerUI: experienceBar 참조가 null입니다!");
    }
    
    // 골드 업데이트
    private void UpdateGold(int amount)
    {
        if (goldText != null)
            goldText.text = $"Gold: {amount}";
    }
    
    private void OnDestroy()
    {
        // 이벤트 구독 해제
        UnsubscribeFromEvents();
    }
    
    private void UnsubscribeFromEvents()
    {
        if (playerState != null)
        {
            playerState.OnLevelChanged -= UpdateLevel;
            playerState.OnExpChanged -= UpdateExperience;
        }
        
        if (playerGold != null)
        {
            playerGold.OnGoldChange -= UpdateGold;
        }
    }
}