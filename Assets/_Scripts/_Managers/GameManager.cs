using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("GameManager 인스턴스가 없습니다!");
            }
            return _instance;
        }
    }

    // 다른 매니저들에 대한 참조
    [SerializeField] private AbilityManager abilityManager;
    // [SerializeField] private UIManager uIManager;
    [SerializeField] private InventoryManager inventoryManager;
    // [SerializeField] private UIinventoryManager uIinventoryManager;

    [SerializeField] private PlayerManager playerManager;
    // 나중에 추가할 매니저들: StageManager, EnemyManager 등등
    [SerializeField] private EquipManager equipManager;
    // 다른 컴포넌트가 접근할 수 있도록 프로퍼티로 노출
    public AbilityManager AbilityManager => abilityManager;
    public InventoryManager InventoryManager => inventoryManager;    
    public PlayerManager PlayerManager => playerManager;
    public EquipManager EquipManager => equipManager;

    // 게임 상태 관련
    public enum GameState { Playing, Paused, GameOver, MainMenu }
    private GameState currentState = GameState.Playing;
    // 상태 변경 이벤트 추가
    public event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        // 싱글톤 패턴 구현
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 매니저들 초기화
        // uIManager.Init();
        // inventoryManager.Init();
        // uIinventoryManager.Init();

    }

    private void Start()
    {
        LoadGameData();
    }

    private void Update()
    {
        // ESC 키 감지 및 처리
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleEscapeKey();
        }
    }

    private void HandleEscapeKey()
    {
        // 현재 상태에 따라 다른 처리
        switch (currentState)
        {
            case GameState.Playing:
                // 플레이 중에 ESC를 누르면 일시정지
                SetGameState(GameState.Paused);
                break;
            case GameState.Paused:
                // 일시정지 상태에서 ESC를 누르면 게임으로 복귀
                SetGameState(GameState.Playing);
                break;
            // 다른 상태에서는 별도 처리 없음
        }
    }

    // 게임 데이터를 언제 저장할지 생각해 봐야함

    #region 저장 및 로드

    public void SaveGameData()
    {
        // 게임 데이터 객체 생성
        GameData data = new GameData();
        
        // 각종 데이터 설정
        // 능력 데이터 저장
        data.unlockedAbilities = abilityManager.GetSaveData();
        
        // 플레이어 위치 저장 (예시)
        // GameObject player = GameObject.FindGameObjectWithTag("Player");
        // if (player != null)
        // {
        //     data.lastPositionX = player.transform.position.x;
        //     data.lastPositionY = player.transform.position.y;
        // }
        
        // 현재 시간 저장
        data.lastSaveTime = DateTime.Now.ToString();
        
        // 데이터를 JSON으로 변환
        string jsonData = JsonUtility.ToJson(data, true);
        
        // 파일로 저장 (PlayerPrefs 대신 파일 시스템 사용)
        string savePath = Application.persistentDataPath + "/gamesave.json";
        System.IO.File.WriteAllText(savePath, jsonData);
        
        Debug.Log("게임 데이터가 저장되었습니다: " + savePath);
    }

    public void LoadGameData()
    {
        string savePath = Application.persistentDataPath + "/gamesave.json";
        
        // 저장 파일이 존재하는지 확인
        if (System.IO.File.Exists(savePath))
        {
            // 파일에서 JSON 데이터 로드
            string jsonData = System.IO.File.ReadAllText(savePath);
            
            // JSON을 GameData 객체로 변환
            GameData data = JsonUtility.FromJson<GameData>(jsonData);
            
            // 데이터 적용
            abilityManager.LoadSaveData(data.unlockedAbilities);
            
            // 플레이어 위치 설정 (예시)
            // GameObject player = GameObject.FindGameObjectWithTag("Player");
            // if (player != null)
            // {
            //     player.transform.position = new Vector3(data.lastPositionX, data.lastPositionY, 0);
            // }
            
            Debug.Log("게임 데이터를 로드했습니다. 마지막 저장: " + data.lastSaveTime);
        }
        else
        {
            Debug.Log("저장된 게임 데이터가 없습니다. 새 게임을 시작합니다.");
            // 필요하다면 초기 게임 데이터 생성
        }
    }

    #endregion

    #region 게임 상태 관리

    public GameState GetCurrentState()
    {
        return currentState;
    }

    public void SetGameState(GameState newState)
    {
        // 동일한 상태로 변경하는 경우 무시
        if (currentState == newState) return;
        
        // 상태 전환 전 후처리
        ExitState(currentState);
        
        // 상태 변경
        GameState oldState = currentState;
        currentState = newState;
        
        // 새 상태에 대한 처리
        EnterState(newState);
        
        // 이벤트 발생
        OnGameStateChanged?.Invoke(newState);
        
        Debug.Log($"게임 상태 변경: {oldState} -> {newState}");
    }

    private void ExitState(GameState state)
    {
        switch (state)
        {
            case GameState.Playing:
                // 플레이 상태 종료 시 처리
                break;
            case GameState.Paused:
                // 일시정지 상태 종료 시 처리
                Time.timeScale = 1f; // 게임 속도 복원
                break;
            case GameState.GameOver:
                // 게임오버 상태 종료 시 처리
                break;
            case GameState.MainMenu:
                // 메인 메뉴 상태 종료 시 처리
                break;
        }
    }

    private void EnterState(GameState state)
    {
        switch (state)
        {
            case GameState.Playing:
                // 플레이 상태 진입 시 처리
                Time.timeScale = 1f; // 게임 속도 정상화
                // UI 숨기기 등
                break;
            case GameState.Paused:
                // 일시정지 상태 진입 시 처리
                Time.timeScale = 0f; // 게임 일시정지
                // 일시정지 메뉴 표시
                break;
            case GameState.GameOver:
                // 게임오버 상태 진입 시 처리
                Time.timeScale = 0f; // 게임 일시정지
                // 게임오버 UI 표시
                break;
            case GameState.MainMenu:
                // 메인 메뉴 상태 진입 시 처리
                Time.timeScale = 1f; // 게임 속도 정상화 (메뉴 애니메이션 등을 위해)
                // 메인 메뉴 UI 표시
                break;
        }
    }

    #endregion

    #region 능력 해제, 스킬 해제 관리
    public void GrantAbility(string abilityName)
    {

    }

    #endregion

    #region 맵 탐색 및 진행 관리
    public void UnlockMap(string mapName)
    {

    }
    #endregion

    #region 업적 관리
    public void UnlockAchievement(string achievementName)
    {

    }
    #endregion
}