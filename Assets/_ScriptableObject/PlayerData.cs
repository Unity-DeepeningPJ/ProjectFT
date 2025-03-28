using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Character/Player")]
public class PlayerData : ScriptableObject
{
    public string playerName;
    public int basePower;
    public int baseDefense;
    public int baseHealth;
    public float baseSpeed;
    public float baseJumpPower;
    public float baseDashDistance;
    public float baseDashSpeed;
    public float baseCriticalChance;

    public int startLevel;
    public int startExp;
    public int expToNextLevel;
}