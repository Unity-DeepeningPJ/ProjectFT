using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[Serializable]
public class HealthBar
{
    // UI 요소
    public Image fillImage;              // 채워지는 이미지
    public TextMeshProUGUI valueText;    // 값 표시 텍스트 (선택적)

    [Header("색상 설정")]
    public Color fullColor = Color.green;       // 가득 찼을 때 색상
    public Color lowColor = Color.red;          // 낮을 때 색상
    public float lowThreshold = 0.3f;           // 낮은 상태로 간주하는 임계값 (0-1)
    public bool useColorGradient = true;        // 그라데이션 색상 사용 여부

    [Header("텍스트 설정")]
    public bool showPercentage = false;         // 퍼센트 표시 여부
    public bool showCurrentMax = true;          // 현재값/최대값 표시
    public bool showDecimal = false;            // 소수점 표시

    // 부드러운 애니메이션을 위한 변수
    private float currentFillAmount = 1f;
    private float targetFillAmount = 1f;

    // 현재 표시된 값 (부드러운 전환을 위해)
    private float currentDisplayValue;
    private float targetValue;
    private float currentMaxValue;

    // 프로그레스 바 값 설정 (0-1 사이)
    public void SetNormalizedValue(float value)
    {
        targetValue = Mathf.Clamp01(value);
    }

    // 현재값과 최대값으로 값 설정
    public void SetValue(float current, float max)
    {
        // 0으로 나누기 방지
        if (max <= 0f) max = 1f;

        // 타겟 값 설정 (0~1 범위로 정규화)
        targetFillAmount = Mathf.Clamp01(current / max);

        // 텍스트 업데이트 (있을 경우)
        if (valueText != null)
        {
            valueText.text = $"{Mathf.Floor(current)}/{Mathf.Floor(max)}";
        }

        currentMaxValue = max;
        targetValue = Mathf.Clamp01(current / max);

        // 텍스트 즉시 업데이트 (부드러운 전환 없이)
        UpdateText(current, max);
    }

    // UI 애니메이션 업데이트 (매 프레임 호출)
    public void UpdateUI(float smoothSpeed = 5f)
    {
        if (fillImage == null) return;

        // 부드러운 변화 적용
        currentFillAmount = Mathf.Lerp(currentFillAmount, targetFillAmount, Time.deltaTime * smoothSpeed);

        // 이미지 채우기 적용
        fillImage.fillAmount = currentFillAmount;

        // 채우기 값 부드럽게 변화
        currentDisplayValue = Mathf.Lerp(currentDisplayValue, targetValue, Time.deltaTime * smoothSpeed);

        // 이미지 채우기 업데이트
        if (fillImage != null)
        {
            fillImage.fillAmount = currentDisplayValue;

            // 색상 업데이트 (낮을 때 빨간색, 높을 때 녹색)
            if (useColorGradient)
            {
                fillImage.color = Color.Lerp(lowColor, fullColor,
                    Mathf.Clamp01(currentDisplayValue / lowThreshold));
            }
        }
    }

    // 텍스트 업데이트
    private void UpdateText(float current, float max)
    {
        if (valueText == null) return;

        if (showCurrentMax)
        {
            if (showDecimal)
                valueText.text = $"{current:F1}/{max:F1}";
            else
                valueText.text = $"{Mathf.RoundToInt(current)}/{Mathf.RoundToInt(max)}";
        }
        else if (showPercentage)
        {
            float percent = (max <= 0) ? 0 : (current / max * 100f);
            valueText.text = showDecimal ? $"{percent:F1}%" : $"{Mathf.RoundToInt(percent)}%";
        }
    }

    public void UpdateMaxValue(float newMaxValue)
    {
        if (newMaxValue <= 0) newMaxValue = 1f;

        // 최대값이 변경되면 현재값을 최대값으로 설정
        targetFillAmount = Mathf.Clamp01(currentDisplayValue / newMaxValue);
        currentMaxValue = newMaxValue;

        // 텍스트 업데이트
        UpdateText(currentDisplayValue, newMaxValue);
    }
}
