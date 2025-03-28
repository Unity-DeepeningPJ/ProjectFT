public class PlayerState : BaseState
{
    public string Name { get; private set; }
    public int Level { get; private set; }
    public int CurrentExp {  get; private set; }
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

    public PlayerState(PlayerData data) : base(data.basePower, data.baseDefense, data.baseHealth, data.baseSpeed, data.baseJumpPower)
    {
        Name = data.playerName;
        Level = data.startLevel;
        CurrentExp = data.startExp;
        ExpToNextLevel = data.expToNextLevel;
        CriticalChance = data.baseCriticalChance;
        DashDistance = data.baseDashDistance;
        DashSpeed = data.baseDashSpeed;

        _equipPower = 0;
        _equipDefense = 0;
        _equipHealth = 0;
        _equipCriticalChance = 0;
    }

    //경험치 추가 및 레벨업
    public void AddExp(int amount)
    {
        CurrentExp += amount;
        if (CurrentExp >= ExpToNextLevel)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        Level++;
        CurrentExp = 0;
        ExpToNextLevel = (int)(ExpToNextLevel * 1.2f);
    }

    //장비 장착, 해제
    public void Equip()
    {

    }

    public void UnEquip()
    {

    }
}
