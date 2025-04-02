using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class TitleParticleEffect : MonoBehaviour
{
    private ParticleSystem particleSystem;
    private Canvas parentCanvas;
    
    [SerializeField] private bool followCamera = true;
    [SerializeField] private float depthOffset = 5f;
    
    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        parentCanvas = GetComponentInParent<Canvas>();
        
        // 캔버스가 Screen Space - Overlay 모드라면
        if (parentCanvas != null && parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            // 메인 카메라 찾기
            Camera mainCamera = Camera.main;
            if (mainCamera != null && followCamera)
            {
                // 파티클을 카메라 앞에 위치시킴
                transform.position = mainCamera.transform.position + mainCamera.transform.forward * depthOffset;
                transform.rotation = Quaternion.identity;
            }
        }
    }
    
    // 필요에 따라 파티클 제어 메서드 추가
    public void SetActive(bool active)
    {
        if (active)
            particleSystem.Play();
        else
            particleSystem.Stop();
    }
}