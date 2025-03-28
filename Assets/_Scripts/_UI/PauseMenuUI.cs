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
        gameManager = GameManager.Instance;
        
        // 처음에 메뉴 비활성화
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        
        // 버튼에 이벤트 연결
        continueButton.onClick.AddListener(ContinueGame);
        settingsButton.onClick.AddListener(OpenSettings);
        mainMenuButton.onClick.AddListener(BackToMainMenu);
        backButton.onClick.AddListener(CloseSettings);
    }

    private void OnEnable()
    {
        // 게임 상태 변경 이벤트 구독
        gameManager.OnGameStateChanged += HandleGameStateChanged;
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
        switch (newState)
        {
            case GameManager.GameState.Playing:
                pauseMenuPanel.SetActive(false);
                settingsPanel.SetActive(false);
                break;
                
            case GameManager.GameState.Paused:
                pauseMenuPanel.SetActive(true);
                settingsPanel.SetActive(false);
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