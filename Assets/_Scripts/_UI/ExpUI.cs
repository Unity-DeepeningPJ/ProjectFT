using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpUI : MonoBehaviour
{
    [Header("UI 요소 참조")]
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI expText;
    
    [Header("애니메이션 설정")]
    [SerializeField] private float animationSpeed = 5f;
    [SerializeField] private Color fillColor = new Color(0.2f, 0.6f, 1f);
    
    // 상태 변수
    private int currentExp;
    private int maxExp;
    private int currentLevel;
    private float displayFillAmount;
    
    private void Start()
    {
        // 필수 UI 요소 확인
        if (fillImage == null)
        {
            Debug.LogError("ExpBar: fillImage가 할당되지 않았습니다!");
        }
        else
        {
            fillImage.color = fillColor;
        }
        
        // UI 초기화
        UpdateUI(false);
    }
    
    private void Update()
    {
        // 부드러운 애니메이션 처리
        if (fillImage != null)
        {
            float targetFillAmount = maxExp > 0 ? (float)currentExp / maxExp : 0;
            if (displayFillAmount != targetFillAmount)
            {
                // 현재 표시값을 목표값으로 부드럽게 보간
                displayFillAmount = Mathf.Lerp(displayFillAmount, targetFillAmount, Time.deltaTime * animationSpeed);
                
                // 매우 근접하면 정확한 값으로 설정
                if (Mathf.Abs(displayFillAmount - targetFillAmount) < 0.005f)
                {
                    displayFillAmount = targetFillAmount;
                }
                
                fillImage.fillAmount = displayFillAmount;
            }
        }
    }
    
    public void SetExp(int current, int max, bool animate = true)
    {
        // 값 변경 로깅
        Debug.Log($"ExpBar - 경험치 설정: {current}/{max}");
        
        // 경험치 값 업데이트
        currentExp = Mathf.Clamp(current, 0, max);
        maxExp = Mathf.Max(1, max); // 0으로 나누기 방지
        
        // UI 업데이트
        UpdateUI(animate);
    }
    
    public void SetLevel(int level)
    {
        currentLevel = level;
        
        // 레벨 텍스트 업데이트
        if (levelText != null)
        {
            levelText.text = $"Lv.{level}";
            Debug.Log($"ExpBar - 레벨 설정: {level}");
        }
    }
    
    private void UpdateUI(bool animate)
    {
        // 애니메이션을 사용하지 않는 경우 즉시 값 적용
        if (!animate && fillImage != null)
        {
            displayFillAmount = maxExp > 0 ? (float)currentExp / maxExp : 0;
            fillImage.fillAmount = displayFillAmount;
        }
        
        // 경험치 텍스트 업데이트
        if (expText != null)
        {
            expText.text = $"{currentExp}/{maxExp}";
        }
    }
    
    public void UpdateAll(int level, int current, int max, bool animate = true)
    {
        SetLevel(level);
        SetExp(current, max, animate);
    }
    
    public void Initialize()
    {
        // UI 요소 참조 체크 및 로깅
        if (fillImage == null) Debug.LogError("ExpBar: fillImage 참조가 누락되었습니다!");
        if (levelText == null) Debug.LogWarning("ExpBar: levelText 참조가 누락되었습니다.");
        if (expText == null) Debug.LogWarning("ExpBar: expText 참조가 누락되었습니다.");
        
        Debug.Log("ExpBar 초기화 완료");
    }
}