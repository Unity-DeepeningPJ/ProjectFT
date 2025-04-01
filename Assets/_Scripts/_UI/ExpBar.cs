using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[Serializable]
public class ExpBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;  // 반드시 할당되어야 함
    [SerializeField] private TextMeshProUGUI expText;  // 선택적
    
    [Header("애니메이션 설정")]
    [SerializeField] private float animationSpeed = 3f;
    [SerializeField] private Color expColor = new Color(0.3f, 0.6f, 1f);
    
    private float currentDisplayValue;
    private int targetValue;
    private int maxValue;

    private void Start()
    {
        fillImage.color = expColor;
        
        if (fillImage == null)
            Debug.LogError("ExpBar: fillImage가 할당되지 않았습니다!");
    }
    
    // 이 메서드가 호출되면 즉시 UI 업데이트가 되어야 함
    public void SetValue(int current, int max)
    {
        targetValue = current;
        maxValue = max;

        // 디버그 추가
        Debug.Log($"ExpBar.SetValue: {current}/{max}");
        
        // 즉시 UI 업데이트
        if (fillImage != null && max > 0)
        {
            fillImage.fillAmount = (float)current / max;
            Debug.Log($"ExpBar fillAmount 설정: {fillImage.fillAmount}");
        }
        else
        {
            Debug.LogError("ExpBar fillImage가 null이거나 max가 0입니다!");
        }
        
        // 텍스트 업데이트 (선택적)
        if (expText != null)
            expText.text = $"{current}/{max}";
    }
    
    // 값이 점진적으로 변하는 것을 원한다면 Update 메서드 필요
    private void Update()
    {
        if (fillImage == null) return;
        
        // 현재 표시값을 목표값으로 부드럽게 이동
        currentDisplayValue = Mathf.Lerp(currentDisplayValue, targetValue, Time.deltaTime * animationSpeed);
        
        // 경험치바 업데이트
        fillImage.fillAmount = maxValue > 0 ? currentDisplayValue / maxValue : 0;
    }
}