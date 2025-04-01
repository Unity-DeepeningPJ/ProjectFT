using System;
using System.Collections.Generic;
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
    [SerializeField] private CurrencyManager currencyManager;
    // 다른 컴포넌트가 접근할 수 있도록 프로퍼티로 노출
    public AbilityManager AbilityManager => abilityManager;
    public InventoryManager InventoryManager => inventoryManager;
    public PlayerManager PlayerManager => playerManager;
    public EquipManager EquipManager => equipManager;
    public CurrencyManager CurrencyManager => currencyManager;

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
            LoadManagers();
        }
        else
        {
            Destroy(gameObject);
            return;
        }

    }

    private void Start()
    {
        LoadGameData();
        StartPlayTimeTracking();
    }

    private void Update()
    {
        // ESC 키 감지 및 처리
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleEscapeKey();
        }
    }

    // private void OnEnable()
    // {
    //     UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    // }

    // private void OnDisable()
    // {
    //     UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    // }

    // private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    // {
    //     Debug.Log($"새 씬 '{scene.name}' 로드됨: 매니저 참조 찾기...");
    //     LoadManagers();
    // }

    public void LoadManagers()
    {
        // "Managers" GameObject 찾기
        GameObject managersObject = GameObject.Find("Managers");

        if (managersObject != null)
        {
            Debug.Log("Managers 게임오브젝트를 찾았습니다.");

            // 해당 오브젝트에서 매니저 컴포넌트 가져오기
            if (abilityManager == null)
                abilityManager = managersObject.GetComponentInChildren<AbilityManager>();

            if (inventoryManager == null)
                inventoryManager = managersObject.GetComponentInChildren<InventoryManager>();

            if (playerManager == null)
                playerManager = managersObject.GetComponentInChildren<PlayerManager>();

            if (equipManager == null)
                equipManager = managersObject.GetComponentInChildren<EquipManager>();

            if (currencyManager == null)
                currencyManager = managersObject.GetComponentInChildren<CurrencyManager>();

            Debug.Log("매니저 컴포넌트 참조 완료");
        }
        else
        {
            Debug.LogWarning("Managers 게임오브젝트를 찾을 수 없습니다!");
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
        // 저장할 데이터 생성
        SaveData data = new SaveData();

        // 플레이어 정보 저장
        if (playerManager != null && playerManager.player != null)
        {
            var player = playerManager.player;
            // data.playerName = player.Name; // Player 클래스에 Name 프로퍼티가 아직 구현되지 않음
            // data.playerLevel = player.Level; // Player 클래스에 Level 프로퍼티가 아직 구현되지 않음
            // 추가 플레이어 정보...            
            // 위치 정보 저장
            data.currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            data.playerPosX = player.transform.position.x;
            data.playerPosY = player.transform.position.y;
        }

        // 인벤토리 정보 저장
        if (inventoryManager != null)
        {
            // 인벤토리 아이템들을 InventoryItemData 리스트로 변환
            // data.inventoryItems = ...
        }

        // 장비 정보 저장
        if (equipManager != null)
        {
            // 현재 장착된 장비 정보 저장
            // data.equippedWeapon = ...
        }

        // 시간 정보 저장
        data.playTimeSeconds = GetTotalPlayTime(); // 플레이 시간 계산 메서드 필요
        data.lastSaveTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        // 현재 슬롯에 저장
        string json = JsonUtility.ToJson(data, true); // true는 pretty print를 의미
        string path = $"{Application.persistentDataPath}/savedata_{currentSlot}.json";
        System.IO.File.WriteAllText(path, json);

        Debug.Log($"슬롯 {currentSlot + 1}에 게임 데이터 저장 완료");
    }

    public void LoadGameData()
    {
        string path = $"{Application.persistentDataPath}/savedata_{currentSlot}.json";

        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            // 플레이어 정보 적용
            if (playerManager != null && playerManager.player != null)
            {
                var player = playerManager.player;
                // 플레이어 데이터 설정
                // player.SetName(data.playerName);
                // player.SetLevel(data.playerLevel);
                // 추가 플레이어 정보...
            }

            // 인벤토리 정보 적용
            if (inventoryManager != null)
            {
                // 인벤토리 초기화 후 저장된 아이템 추가
                // inventoryManager.ClearInventory();
                // foreach (var itemData in data.inventoryItems) { ... }
            }

            // 장비 정보 적용
            if (equipManager != null)
            {
                // 저장된 장비 장착
                // equipManager.EquipItem(data.equippedWeapon);
                // ...
            }

            // 시간 정보 적용
            // SetTotalPlayTime(data.playTimeSeconds);

            Debug.Log($"슬롯 {currentSlot + 1}에서 게임 데이터 로드 완료");
        }
        else
        {
            Debug.LogWarning($"슬롯 {currentSlot + 1}에 저장 데이터가 없습니다.");
        }
    }

    public bool HasSaveDataForSlot(int slotIndex)
    {
        string path = $"{Application.persistentDataPath}/savedata_{slotIndex}.json";
        return System.IO.File.Exists(path);
    }

    public bool HasSaveData()
    {
        for (int i = 0; i < 4; i++)
        {
            if (HasSaveDataForSlot(i))
                return true;
        }
        return false;
    }

    public SaveSlotData GetSaveSlotInfo(int slotIndex)
    {
        SaveSlotData data = new SaveSlotData();

        // 파일이 존재하는지 확인
        string path = $"{Application.persistentDataPath}/savedata_{slotIndex}.json";
        if (System.IO.File.Exists(path))
        {
            try
            {
                // 파일에서 기본 정보만 읽기 (전체 데이터를 읽지 않고 필요한 정보만)
                string json = System.IO.File.ReadAllText(path);
                SaveData fullData = JsonUtility.FromJson<SaveData>(json);

                // 메타데이터만 추출
                data.playerLevel = fullData.playerLevel;
                data.playerName = fullData.playerName;
                data.playTime = FormatPlayTime(fullData.playTimeSeconds);
                data.lastSaveTime = fullData.lastSaveTime;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"슬롯 {slotIndex} 데이터 읽기 오류: {e.Message}");
            }
        }

        return data;
    }

    public void LoadGameDataFromSlot(int slotIndex)
    {
        currentSlot = slotIndex;
        LoadGameData();
    }

    private int currentSlot = 0;
    public void SetCurrentSlot(int slotIndex)
    {
        currentSlot = slotIndex;
    }

    private string FormatPlayTime(int seconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(seconds);
        return string.Format("{0:D2}:{1:D2}:{2:D2}", time.Hours, time.Minutes, time.Seconds);
    }

    public void ResetGameData()
    {
        // 게임 데이터 초기화 로직
        Debug.Log("게임 데이터 초기화");

        // 플레이어 스탯 초기화
        // 게임 진행 초기화
        // 등등...
    }

    private int totalPlayTime = 0;
    private float sessionStartTime;

    public void StartPlayTimeTracking()
    {
        sessionStartTime = Time.time;
    }

    private int GetTotalPlayTime()
    {
        // 현재 세션 플레이 시간 계산
        float currentSessionTime = Time.time - sessionStartTime;
        return totalPlayTime + Mathf.FloorToInt(currentSessionTime);
    }

    private void SetTotalPlayTime(int seconds)
    {
        totalPlayTime = seconds;
        sessionStartTime = Time.time; // 새 세션 시작
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

[System.Serializable]
public class SaveSlotData
{
    public string playerName = "";
    public int playerLevel = 1;
    public string playTime = "00:00:00";
    public string lastSaveTime = "";
}

[System.Serializable]
public class SaveData
{
    // 플레이어 정보
    public string playerName = "Player";
    public int playerLevel = 1;
    public int playerCurrentExp = 0;
    public int playerExpToNextLevel = 100;
    public float playerMaxHealth = 100f;
    public float playerCurrentHealth = 100f;
    public int playerPower = 10;
    public int playerDefense = 5;

    // 위치 정보
    public string currentSceneName = "";
    public float playerPosX = 0f;
    public float playerPosY = 0f;

    // 게임 진행 정보
    public int gold = 0;
    public List<string> unlockedAbilities = new List<string>();
    public List<string> completedQuests = new List<string>();
    public List<string> unlockedMaps = new List<string>();
    public List<string> unlockedAchievements = new List<string>();

    // 인벤토리 정보
    public List<InventoryItemData> inventoryItems = new List<InventoryItemData>();

    // 장비 정보
    public string equippedWeapon = "";
    public string equippedArmor = "";
    public string equippedAccessory = "";

    // 시간 정보
    public int playTimeSeconds = 0;
    public string lastSaveTime = "";

    // 추가적인 필드...
}

[System.Serializable]
public class InventoryItemData
{
    public string itemId;
    public int amount;
    public int slotIndex;

    public InventoryItemData(string id, int count, int slot)
    {
        itemId = id;
        amount = count;
        slotIndex = slot;
    }
}