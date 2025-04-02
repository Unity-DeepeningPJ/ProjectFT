using UnityEngine;
public interface INPC
{
    string NPCName { get; } // NPC 이름
    Sprite NPCPortrait { get; } // NPC 초상화
    bool IsInteractable { get; } // 상호작용 가능 여부
    void Interact(PlayerController player); // 상호작용 메서드
    void UpdateState(GameManager.GameState gameState); // 게임 상태 변경시 호출
}