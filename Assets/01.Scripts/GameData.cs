using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    // 플레이어 능력 관련 데이터
    public List<string> unlockedAbilities = new List<string>();
    
    // 플레이어 스탯 관련 데이터
    // 플레이어를 참조해서 가져오면 될 듯?

    
    // 맵 탐색 관련 데이터
    public List<string> visitedAreas = new List<string>();
    public List<string> unlockedDoors = new List<string>();
    
    // 마지막 저장 위치
    public float lastPositionX;
    public float lastPositionY;
    
    // 마지막 저장 시간
    public string lastSaveTime;
    
    // 업적 관련 데이터
    public List<string> unlockedAchievements = new List<string>();
    
    // 기타 필요한 게임 데이터를 여기에 추가
}