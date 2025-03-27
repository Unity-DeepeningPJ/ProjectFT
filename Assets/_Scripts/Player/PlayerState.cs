public class PlayerState : BaseState
{
    public string Name { get; private set; }
    public int Level { get; private set; }
    public int CurrentExp {  get; private set; }
    public int ExpToNextLevel { get; private set; }

    public float CriticalChance { get; private set; }

    private int _equipPower;
    private int _equipDefense;
    private int _equipHealth;
    private float _equipCriticalChance;

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

        _equipPower = 0;
        _equipDefense = 0;
        _equipHealth = 0;
        _equipCriticalChance = 0;
    }

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

    public void Equip()
    {

    }

    public void UnEquip()
    {

    }
}
