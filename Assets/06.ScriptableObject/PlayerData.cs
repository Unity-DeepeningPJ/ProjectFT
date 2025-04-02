using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Character/Player")]
public class PlayerData : ScriptableObject
{
    public string playerName;
    public int startLevel;
    public int startExp;
    public int expToNextLevel;

    //플레이어 스탯
    public int basePower;   //공격력
    public int baseDefense; //방어력
    public int baseHealth;  //체력
    public float baseCriticalChance;    //치명타

    //이동 스탯
    public float baseSpeed;     //이동 속도
    public float baseJumpPower; //점프력
    public float baseDashDistance;  //대쉬 거리
    public float baseDashSpeed; //대쉬 속도
}


