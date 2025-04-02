using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Game/Ability")]
public class AbilityData : ScriptableObject
{
    public string abilityID;           // 고유 ID (예: "DOUBLE_JUMP")
    public string abilityName;         // 표시 이름 (예: "더블 점프")
    public string description;         // 설명
    public Sprite icon;                // UI 표시용 아이콘
    
    [Header("획득 관련")]
    public string unlockLocation;      // 획득 위치/방법 설명
    public bool isDefaultAbility;      // 기본 보유 능력인지 여부
    
    [Header("기능 관련")]
    public bool isPassive;             // 패시브(항상 적용) 또는 액티브(버튼 입력 필요)
    public string inputButtonName;     // 액티브 능력의 경우 입력 버튼 이름
    public float cooldownTime;         // 재사용 대기시간 (액티브 능력)
    
    [Header("시각/사운드 효과")]
    public GameObject visualEffectPrefab; // 능력 사용 시 시각 효과
    public AudioClip soundEffect;         // 능력 사용 시 사운드 효과
}