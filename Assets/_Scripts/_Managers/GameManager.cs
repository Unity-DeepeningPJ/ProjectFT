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
    // [SerializeField] private UIManager uIManager;
    public InventoryManager inventoryManager;
    // [SerializeField] private UIinventoryManager uIinventoryManager;

    // 나중에 추가할 매니저들: StageManager, EnemyManager 등등

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

    }

    public void LoadGameData()
    {

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