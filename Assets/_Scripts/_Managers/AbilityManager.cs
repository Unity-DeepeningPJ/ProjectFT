using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    [SerializeField] private List<AbilityData> allAbilities = new List<AbilityData>(); // 모든 능력 데이터
    
    [SerializeField] private List<PlayerAbility> playerAbilities = new List<PlayerAbility>(); // 플레이어 능력 목록
    
    // 이벤트 정의
    public event Action<string> OnAbilityUnlocked; // 능력 해금 시 발생
    
    [System.Serializable]
    public class PlayerAbility
    {
        public string abilityID; // 능력 ID
        public bool isUnlocked; // 능력 해금 여부
        public float lastUsedTime = -999f; // 마지막 사용 시간
    }
    
    private void Start()
    {
        InitializeAbilities(); // 능력 초기화
    }
    
    private void InitializeAbilities()
    {
        // 아직 능력 리스트가 초기화되지 않았다면 초기화
        if (playerAbilities.Count == 0)
        {
            foreach (AbilityData ability in allAbilities)
            {
                playerAbilities.Add(new PlayerAbility
                {
                    abilityID = ability.abilityID, // 능력 ID
                    isUnlocked = ability.isDefaultAbility, // 기본 능력인지 여부
                    lastUsedTime = -999f // 마지막 사용 시간
                });
            }
        }
    }
    
    // 새 능력 해금
    public void GrantAbility(string abilityName)
    {
        // 능력 ID 찾기
        PlayerAbility ability = playerAbilities.Find(a => a.abilityID == abilityName);
        
        if (ability == null)
        {
            Debug.LogError($"존재하지 않는 능력입니다: {abilityName}");
            return;
        }
        
        if (ability.isUnlocked)
        {
            Debug.Log($"이미 해금된 능력입니다: {abilityName}");
            return;
        }
        
        // 능력 해금하기
        ability.isUnlocked = true;
        
        // 능력 데이터 가져오기
        AbilityData abilityData = allAbilities.Find(a => a.abilityID == abilityName);
        
        // 획득 효과 표시 (UI 연동 등)
        Debug.Log($"새로운 능력을 획득했습니다: {abilityData.abilityName}");
        
        // 이벤트 발생
        OnAbilityUnlocked?.Invoke(abilityName);
    }
    
    // 능력 사용 가능 여부 확인
    public bool CanUseAbility(string abilityName)
    {
        PlayerAbility ability = playerAbilities.Find(a => a.abilityID == abilityName);
        
        if (ability == null || !ability.isUnlocked)
            return false;
        
        AbilityData abilityData = allAbilities.Find(a => a.abilityID == abilityName);
        
        // 패시브 능력은 항상 사용 가능
        if (abilityData.isPassive)
            return true;
        
        // 액티브 능력은 쿨다운 체크
        return Time.time >= ability.lastUsedTime + abilityData.cooldownTime;
    }
    
    // 능력 사용 (액티브 능력용)
    public void UseAbility(string abilityName)
    {
        if (!CanUseAbility(abilityName))
            return;
        
        PlayerAbility ability = playerAbilities.Find(a => a.abilityID == abilityName);
        AbilityData abilityData = allAbilities.Find(a => a.abilityID == abilityName);
        
        // 쿨다운 업데이트
        ability.lastUsedTime = Time.time;
        
        // 필요한 효과 표시 로직 파이클이나 사운드??
    }
    
    // 능력 해금 여부 확인
    public bool HasAbility(string abilityName)
    {
        PlayerAbility ability = playerAbilities.Find(a => a.abilityID == abilityName);
        return ability != null && ability.isUnlocked; // 능력이 존재하고 해금되었는지 여부 반환
    }
    
    // 모든 획득 능력 목록 반환 (UI 표시용)
    public List<AbilityData> GetUnlockedAbilities() // UI 표시용
    {
        List<AbilityData> unlockedAbilities = new List<AbilityData>(); // 해금된 능력 목록
        
        foreach (PlayerAbility playerAbility in playerAbilities) // 모든 플레이어 능력 순회
        {
            if (playerAbility.isUnlocked)
            {
                AbilityData data = allAbilities.Find(a => a.abilityID == playerAbility.abilityID);
                if (data != null)
                    unlockedAbilities.Add(data); // 해금된 능력 목록에 추가
            }
        }
        
        return unlockedAbilities;
    }
    
    // 저장 및 로드 기능
    public List<string> GetSaveData()
    {
        List<string> unlockedAbilityIDs = new List<string>(); // 해금된 능력 ID 목록
        foreach (var ability in playerAbilities) 
        {
            if (ability.isUnlocked) // 능력이 해금되었다면
                unlockedAbilityIDs.Add(ability.abilityID); // ID 목록에 추가
        }
        return unlockedAbilityIDs; // ID 목록 반환
    }
    
    public void LoadSaveData(List<string> unlockedAbilityIDs) // 저장된 데이터로 능력 해금
    {
        if (unlockedAbilityIDs == null) return; // 저장된 데이터가 없다면 종료
        
        foreach (string abilityID in unlockedAbilityIDs) // 저장된 능력 ID 목록 순회
        {
            var ability = playerAbilities.Find(a => a.abilityID == abilityID); // 능력 찾기
            if (ability != null) // 능력이 존재한다면
                ability.isUnlocked = true; // 능력 해금
        }
    }
}