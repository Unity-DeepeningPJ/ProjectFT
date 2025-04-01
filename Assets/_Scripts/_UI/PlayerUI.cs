// PlayerUI.cs - 플레이어 UI 전체를 관리하는 메인 클래스
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [Header("체력 관련")]
    [SerializeField] private HealthUI healthUI;
    
    [Header("레벨과 경험치")]
    [SerializeField] private ExpUI expUI; 
    
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
        
        // UI 초기화
        InitializeUIComponents();
        
        // 이벤트 구독
        SubscribeToEvents();
    }
    
    private void Update()
    {
        // HealthUI 애니메이션 업데이트 - 프레임마다 호출해야 함
        healthUI.UpdateUI(5f); // 애니메이션 속도 5로 설정
    }
    
    private void InitializeUIComponents()
    {
        // 체력 UI 초기화
        if (playerCondition != null && playerState != null)
        {
            Debug.Log($"체력 UI 초기화: {playerCondition.CurrentHealth}/{playerState.TotalHealth}");
            healthUI.SetValue(playerCondition.CurrentHealth, playerState.TotalHealth);
        }
        else
        {
            Debug.LogWarning("PlayerUI: playerCondition 또는 playerState가 null입니다!");
        }
        
        // ExpBar 초기화
        if (expUI != null)
        {
            expUI.Initialize();
            
            // 초기값 설정
            if (playerState != null)
            {
                expUI.UpdateAll
                (
                    playerState.Level,
                    playerState.CurrentExp, 
                    playerState.ExpToNextLevel
                );
            }
        }
        
        // 골드 텍스트 초기화
        if (playerGold != null && goldText != null)
        {
            goldText.text = $"Gold: {playerGold.currencies[CurrenyType.Gold]}";
        }
    }
    
    private void SubscribeToEvents()
    {
        // 체력 관련 이벤트 구독
        if (playerCondition != null)
        {
            playerCondition.OnHealthChanged += UpdateHealthFromCondition;
            Debug.Log("PlayerUI: 플레이어 컨디션 체력 이벤트 구독");
        }
        
        if (playerState != null)
        {
            playerState.OnHealthChanged += UpdateHealth;
            playerState.OnMaxHealthChanged += UpdateMaxHealth;
            playerState.OnLevelChanged += OnLevelChanged;
            playerState.OnExpChanged += OnExpChanged;
            Debug.Log("PlayerUI: 플레이어 스테이트 이벤트 구독");
        }
        
        if (playerGold != null)
        {
            playerGold.OnGoldChange += UpdateGold;
        }
    }
    
    // PlayerCondition의 체력 변화 처리
    private void UpdateHealthFromCondition(float current)
    {
        Debug.Log($"체력 업데이트(컨디션): {current}/{playerState?.TotalHealth}");
        if (playerState != null)
        {
            healthUI.SetValue(current, playerState.TotalHealth);
        }
    }
    
    // PlayerState의 체력 변화 처리
    private void UpdateHealth(float current, float max)
    {
        Debug.Log($"체력 업데이트(스테이트): {current}/{max}");
        healthUI.SetValue(current, max);
    }
    
    // 최대 체력 변경 처리
    private void UpdateMaxHealth(float newMaxHealth)
    {
        Debug.Log($"최대 체력 업데이트: {newMaxHealth}");
        healthUI.UpdateMaxValue(playerCondition.CurrentHealth, newMaxHealth);
    }
    
    // 경험치 이벤트 핸들러
    private void OnExpChanged(int current, int max)
    {
        if (expUI != null)
            expUI.SetExp(current, max);
    }

    // 레벨 이벤트 핸들러
    private void OnLevelChanged(int newLevel)
    {
        if (expUI != null)
            expUI.SetLevel(newLevel);
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
        // 체력 이벤트 구독 해제 추가
        if (playerCondition != null)
        {
            playerCondition.OnHealthChanged -= UpdateHealthFromCondition;
        }
        
        if (playerState != null)
        {
            playerState.OnHealthChanged -= UpdateHealth;
            playerState.OnMaxHealthChanged -= UpdateMaxHealth;
            playerState.OnLevelChanged -= OnLevelChanged;
            playerState.OnExpChanged -= OnExpChanged;
        }
        
        if (playerGold != null)
        {
            playerGold.OnGoldChange -= UpdateGold;
        }
    }
}