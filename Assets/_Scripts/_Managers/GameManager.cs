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
    public InventoryManager inventoryManager;
    // [SerializeField] private UIinventoryManager uIinventoryManager;

    // 나중에 추가할 매니저들: StageManager, EnemyManager 등등

    // 다른 컴포넌트가 AbilityManager에 접근할 수 있도록 프로퍼티로 노출
    public AbilityManager AbilityManager => abilityManager;
    
    // 게임 상태 관련
    public enum GameState { Playing, Paused, GameOver, MainMenu }

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

    public void SetGameState(GameState newState)
    {

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