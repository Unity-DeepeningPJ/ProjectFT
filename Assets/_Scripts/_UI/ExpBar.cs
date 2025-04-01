using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[Serializable]
public class ExpBar
{
    // UI 요소
    public Image fillImage;              // 채워지는 이미지
    public TextMeshProUGUI expText;      // 경험치 텍스트
    
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
    
    public void SetValue(int current, int max)
    {
        targetValue = current;
        maxValue = max;
        
        // 텍스트 업데이트
        if (expText != null)
            expText.text = $"{current}/{max}";
    }
    
    public void UpdateUI()
    {
        if (fillImage == null) return;
        
        // 현재 표시값을 목표값으로 부드럽게 이동
        currentDisplayValue = Mathf.Lerp(currentDisplayValue, targetValue, Time.deltaTime * animationSpeed);
        
        // 경험치바 업데이트
        fillImage.fillAmount = maxValue > 0 ? currentDisplayValue / maxValue : 0;
    }
}