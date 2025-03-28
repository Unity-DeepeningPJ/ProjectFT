using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    [Header("패널 참조")]
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("버튼 참조")]
    [SerializeField] private Button continueButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button backButton; // 설정 화면에서 뒤로 가기

    private GameManager gameManager;

    private void Awake()
    {
        // GameManager 인스턴스 가져오기
        gameManager = GameManager.Instance;
        Debug.Log($"PauseMenuUI Awake: GameManager 참조 {(gameManager != null ? "성공" : "실패")}");
        
        // 처음에 메뉴 비활성화
        if (pauseMenuPanel != null) 
        {
            pauseMenuPanel.SetActive(false);
            Debug.Log("PauseMenuUI: 일시정지 메뉴 패널 비활성화");
        }
        else
        {
            Debug.LogError("PauseMenuUI: pauseMenuPanel이 할당되지 않았습니다!");
        }
        
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
            Debug.Log("PauseMenuUI: 설정 패널 비활성화");
        }
        else
        {
            Debug.LogError("PauseMenuUI: settingsPanel이 할당되지 않았습니다!");
        }
        
        // 버튼에 이벤트 연결
        if (continueButton != null)
            continueButton.onClick.AddListener(ContinueGame);
        else
            Debug.LogError("PauseMenuUI: continueButton이 할당되지 않았습니다!");
            
        if (settingsButton != null)
            settingsButton.onClick.AddListener(OpenSettings);
        else
            Debug.LogError("PauseMenuUI: settingsButton이 할당되지 않았습니다!");
            
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(BackToMainMenu);
        else
            Debug.LogError("PauseMenuUI: mainMenuButton이 할당되지 않았습니다!");
            
        if (backButton != null)
            backButton.onClick.AddListener(CloseSettings);
        else
            Debug.LogError("PauseMenuUI: backButton이 할당되지 않았습니다!");
    }

    private void OnEnable()
    {
        // 게임 상태 변경 이벤트 구독
        if (gameManager != null)
        {
            gameManager.OnGameStateChanged += HandleGameStateChanged;
            Debug.Log("PauseMenuUI: 게임 상태 변경 이벤트 구독");
        }
        else
        {
            Debug.LogError("PauseMenuUI OnEnable: GameManager 참조가 null입니다!");
        }
    }

    private void OnDisable()
    {
        // 이벤트 구독 해제
        if (gameManager != null)
        {
            gameManager.OnGameStateChanged -= HandleGameStateChanged;
        }
    }

    // 게임 상태 변경 처리
    private void HandleGameStateChanged(GameManager.GameState newState)
    {
        Debug.Log($"PauseMenuUI: 게임 상태 변경 감지 - {newState}");
        
        switch (newState)
        {
            case GameManager.GameState.Playing:
                pauseMenuPanel.SetActive(false);
                settingsPanel.SetActive(false);
                Debug.Log("PauseMenuUI: 게임 플레이 모드 - UI 숨김");
                break;
                
            case GameManager.GameState.Paused:
                pauseMenuPanel.SetActive(true);
                settingsPanel.SetActive(false);
                Debug.Log("PauseMenuUI: 일시정지 모드 - 메뉴 패널 표시");
                break;
                
            case GameManager.GameState.MainMenu:
                // 씬 전환은 GameManager에서 처리
                break;
        }
    }

    // 계속하기 버튼
    private void ContinueGame()
    {
        gameManager.SetGameState(GameManager.GameState.Playing);
    }
    
    // 설정 열기
    private void OpenSettings()
    {
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }
    
    // 설정 닫기
    private void CloseSettings()
    {
        pauseMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }
    
    // 메인 메뉴로 돌아가기
    private void BackToMainMenu()
    {
        // 필요한 데이터 저장
        gameManager.SaveGameData();
        
        // 상태 변경 및 씬 로드 요청
        gameManager.SetGameState(GameManager.GameState.MainMenu);
        SceneManager.LoadScene("MainMenu"); // 메인 메뉴 씬 이름에 맞게 수정 필요
    }
}