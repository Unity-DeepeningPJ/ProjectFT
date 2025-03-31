using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class TitleUI : MonoBehaviour
{
    [Header("패널 참조")]
    [SerializeField] private GameObject mainMenuPanel;     // 메인 메뉴 패널
    [SerializeField] private GameObject saveSlotPanel;     // 저장 슬롯 패널 (추가)
    [SerializeField] private GameObject settingsPanel;     // 설정 패널
    [SerializeField] private GameObject achievementsPanel; // 업적 패널
    [SerializeField] private GameObject morePanel;         // 더보기 패널

    [Header("버튼 참조")]
    [SerializeField] private Button startGameButton;       // 게임 시작 버튼
    [SerializeField] private Button settingsButton;        // 설정 버튼
    [SerializeField] private Button achievementsButton;    // 업적 버튼
    [SerializeField] private Button moreButton;            // 더보기 버튼
    [SerializeField] private Button quitButton;            // 게임 종료 버튼
    
    [Header("뒤로가기 버튼들")]
    [SerializeField] private Button settingsBackButton;    // 설정에서 뒤로가기
    [SerializeField] private Button achievementsBackButton; // 업적에서 뒤로가기
    [SerializeField] private Button moreBackButton;        // 더보기에서 뒤로가기
    [SerializeField] private Button saveSlotBackButton;    // 저장 슬롯에서 뒤로가기 (추가)
    
    [Header("저장 슬롯 참조")]
    [SerializeField] private Button[] saveSlotButtons = new Button[4]; // 4개의 슬롯 버튼
    [SerializeField] private TextMeshProUGUI[] slotInfoTexts = new TextMeshProUGUI[4]; // 슬롯 정보 텍스트
    
    [Header("제목 설정")]
    [SerializeField] private TextMeshProUGUI titleText;    // 게임 제목 텍스트
    [SerializeField] private string gameTitle = "Project FT";
    
    [Header("버전 정보")]
    [SerializeField] private TextMeshProUGUI versionText;  // 버전 텍스트
    [SerializeField] private string versionNumber = "v0.1";

    private GameManager gameManager;
    
    private void Awake()
    {
        // GameManager 참조 가져오기
        gameManager = GameManager.Instance;
        
        // 초기 UI 설정
        InitializeUI();
        
        // 버튼 이벤트 설정
        SetupButtons();
    }
    
    private void Start()
    {
        // 게임 상태를 MainMenu로 설정
        if (gameManager != null)
        {
            gameManager.SetGameState(GameManager.GameState.MainMenu);
        }
        
        // 배경 음악 재생 (있다면)
        PlayTitleMusic();
    }
    
    private void InitializeUI()
    {
        // 메인 메뉴 패널만 활성화
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (saveSlotPanel != null) saveSlotPanel.SetActive(false); // 추가
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (achievementsPanel != null) achievementsPanel.SetActive(false);
        if (morePanel != null) morePanel.SetActive(false);
        
        // 제목과 버전 텍스트 설정
        if (titleText != null) titleText.text = gameTitle;
        if (versionText != null) versionText.text = versionNumber;
    }
    
    private void SetupButtons()
    {
        // 메인 메뉴 버튼 이벤트 연결
        if (startGameButton != null) startGameButton.onClick.AddListener(ShowSaveSlots); // 변경
        if (settingsButton != null) settingsButton.onClick.AddListener(OpenSettings);
        if (achievementsButton != null) achievementsButton.onClick.AddListener(OpenAchievements);
        if (moreButton != null) moreButton.onClick.AddListener(OpenMore);
        if (quitButton != null) quitButton.onClick.AddListener(QuitGame);
        
        // 저장 슬롯 버튼 이벤트 연결 (추가)
        for (int i = 0; i < saveSlotButtons.Length; i++)
        {
            int slotIndex = i; // 클로저를 위한 로컬 변수
            if (saveSlotButtons[i] != null)
                saveSlotButtons[i].onClick.AddListener(() => SelectSaveSlot(slotIndex));
        }
        
        // 저장 슬롯 뒤로가기 버튼 (추가)
        if (saveSlotBackButton != null)
            saveSlotBackButton.onClick.AddListener(CloseSaveSlots);
        
        // 다른 뒤로가기 버튼들
        if (settingsBackButton != null) settingsBackButton.onClick.AddListener(CloseSettings);
        if (achievementsBackButton != null) achievementsBackButton.onClick.AddListener(CloseAchievements);
        if (moreBackButton != null) moreBackButton.onClick.AddListener(CloseMore);
    }
    
    // 저장 슬롯 보여주기 (새로운 메서드)
    private void ShowSaveSlots()
    {
        Debug.Log("저장 슬롯 패널 표시");
        mainMenuPanel.SetActive(false);
        saveSlotPanel.SetActive(true);
        
        // 저장 슬롯 정보 업데이트
        UpdateSaveSlotInfo();
    }
    
    // 저장 슬롯 정보 업데이트 (새로운 메서드)
    private void UpdateSaveSlotInfo()
    {
        for (int i = 0; i < 4; i++)
        {
            bool hasData = false;
            string slotInfo = "";
            
            // GameManager를 통해 각 슬롯의 저장 데이터 확인
            if (gameManager != null)
            {
                hasData = gameManager.HasSaveDataForSlot(i);
                
                if (hasData)
                {
                    // 저장 데이터가 있는 경우 정보 표시 (레벨, 플레이 시간 등)
                    SaveSlotData data = gameManager.GetSaveSlotInfo(i);
                    slotInfo = $"Lv.{data.playerLevel} - {data.playTime}";
                    
                    // 버튼 텍스트를 "이어하기"로 변경
                    TextMeshProUGUI buttonText = saveSlotButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                    if (buttonText != null) buttonText.text = "이어하기";
                }
                else
                {
                    // 빈 슬롯인 경우
                    slotInfo = "비어 있음";
                    
                    // 버튼 텍스트를 "새 게임"으로 변경
                    TextMeshProUGUI buttonText = saveSlotButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                    if (buttonText != null) buttonText.text = "새 게임";
                }
            }
            else
            {
                slotInfo = "데이터 없음";
            }
            
            // UI에 슬롯 정보 표시
            if (slotInfoTexts[i] != null)
            {
                slotInfoTexts[i].text = slotInfo;
            }
        }
    }
    
    // 저장 슬롯 선택 처리 (새로운 메서드)
    private void SelectSaveSlot(int slotIndex)
    {
        Debug.Log($"슬롯 {slotIndex+1} 선택됨");
        
        bool hasData = false;
        if (gameManager != null)
        {
            hasData = gameManager.HasSaveDataForSlot(slotIndex);
        }
        
        if (hasData)
        {
            // 저장된 게임 데이터가 있으면 불러오기
            if (gameManager != null)
            {
                gameManager.LoadGameDataFromSlot(slotIndex);
            }
        }
        else
        {
            // 빈 슬롯이면 새 게임 시작
            if (gameManager != null)
            {
                gameManager.SetCurrentSlot(slotIndex);
                gameManager.ResetGameData();
            }
        }
        
        // 게임 씬 로드
        LoadGameScene();
    }
    
    // 저장 슬롯 패널 닫기 (새로운 메서드)
    private void CloseSaveSlots()
    {
        saveSlotPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
    
    // 설정 패널 열기
    private void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }
    
    // 설정 패널 닫기
    private void CloseSettings()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
    
    // 업적 패널 열기
    private void OpenAchievements()
    {
        mainMenuPanel.SetActive(false);
        achievementsPanel.SetActive(true);
        
        // 업적 데이터 로드
        LoadAchievements();
    }
    
    // 업적 데이터 로드
    private void LoadAchievements()
    {
        // 게임 매니저로부터 업적 데이터 로드
        if (gameManager != null)
        {
            // gameManager.LoadAchievements();
            Debug.Log("업적 데이터 로드");
        }
    }
    
    // 업적 패널 닫기
    private void CloseAchievements()
    {
        achievementsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
    
    // 더보기 패널 열기
    private void OpenMore()
    {
        mainMenuPanel.SetActive(false);
        morePanel.SetActive(true);
    }
    
    // 더보기 패널 닫기
    private void CloseMore()
    {
        morePanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
    
    // 게임 종료
    private void QuitGame()
    {
        Debug.Log("게임 종료");
        
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
    
    // 게임 씬 로드
    private void LoadGameScene()
    {
        // 게임 씬 로드
        SceneManager.LoadScene("Main"); // 실제 게임 씬 이름으로 변경
        // 게임상태를 Playing으로 설정
        if (gameManager != null)
        {
            gameManager.SetGameState(GameManager.GameState.Playing);
        }
        Debug.Log("게임 씬 로드");
    }
    
    // 타이틀 음악 재생
    private void PlayTitleMusic()
    {
        // 오디오 매니저가 있다면 타이틀 음악 재생
        // AudioManager.Instance.PlayMusic("TitleTheme");
        Debug.Log("타이틀 음악 재생");
    }
}