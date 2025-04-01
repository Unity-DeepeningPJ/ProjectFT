using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class PlayerState : BaseState
{
    public string Name { get; private set; }
    public int Level { get; private set; }
    public int CurrentExp { get; private set; }
    public int ExpToNextLevel { get; private set; }

    public float CriticalChance { get; private set; }

    public float DashDistance { get; private set; }
    public float DashSpeed { get; private set; }

    //장비로 인한 추가 스탯
    private int _equipPower;
    private int _equipDefense;
    private int _equipHealth;
    private float _equipCriticalChance;

    //최종 스탯
    public int TotalPower => Power + _equipPower;
    public int TotalDefense => Defense + _equipDefense;
    public int TotalHealth => health + _equipHealth;
    public float TotalCriticalChance => CriticalChance + _equipCriticalChance;

    public event Action<PlayerState> OnStatsChanged;
    public event Action<float, float> OnHealthChanged;  // 현재 체력, 최대 체력
    public event Action<float> OnMaxHealthChanged;  // 최대 체력
    public event Action<int> OnLevelChanged;  // 새 레벨
    public event Action<int, int> OnExpChanged;  // 현재 경험치, 다음 레벨까지 필요한 경험치

    public PlayerState(PlayerData data) : base(data.basePower, data.baseDefense, data.baseHealth, data.baseSpeed, data.baseJumpPower) { }

    public void Initialize(PlayerData data)
    {
        Name = data.playerName;
        Level = data.startLevel;
        CurrentExp = data.startExp;
        ExpToNextLevel = data.expToNextLevel;

        Power = data.basePower;
        Defense = data.baseDefense;
        health = data.baseHealth;
        CriticalChance = data.baseCriticalChance;

        speed = data.baseSpeed;
        jumpPower = data.baseJumpPower;
        DashDistance = data.baseDashDistance;
        DashSpeed = data.baseDashSpeed;

        _equipPower = 0;
        _equipDefense = 0;
        _equipHealth = 0;
        _equipCriticalChance = 0;
    }

    //장비 스텟 변경 메서드
    public void UpdateEquipStats(int power, int defense, int health, float critical)
    {
        _equipPower = power;
        _equipDefense = defense;
        _equipHealth = health;
        _equipCriticalChance = critical;

        OnStatsChanged?.Invoke(this);
    }

    //경험치 추가 및 레벨업
    public void AddExp(int amount)
    {
        Debug.Log($"CurrentExp: {CurrentExp}, ExpToNextLevel: {ExpToNextLevel}");
        CurrentExp += amount;
        OnExpChanged?.Invoke(CurrentExp, ExpToNextLevel);

        if (CurrentExp >= ExpToNextLevel)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        Debug.Log($"Level Up! New Level: {Level + 1}");
        Debug.Log($"CurrentExp: {CurrentExp}, ExpToNextLevel: {ExpToNextLevel}");
        Level++;
        CurrentExp = 0;
        ExpToNextLevel = (int)(ExpToNextLevel * 1.2f);
        health += 10;
        Power += 1;
        Defense += 1;

        // 이벤트 발생
        OnLevelChanged?.Invoke(Level);
        OnExpChanged?.Invoke(CurrentExp, ExpToNextLevel);
        OnMaxHealthChanged?.Invoke(TotalHealth);
        OnStatsChanged?.Invoke(this);
    }
}
