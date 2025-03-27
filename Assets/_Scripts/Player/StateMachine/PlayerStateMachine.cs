using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public Player Player { get; }

    public Vector2 MoveInput { get; set; }

    public Transform MainCamTransform;

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }

    public PlayerStateMachine(Player player)
    {
        this.Player = player;
        MainCamTransform = Camera.main.transform;

        IdleState = new PlayerIdleState(this);
        MoveState = new PlayerMoveState(this);
        DashState = new PlayerDashState(this);
        JumpState = new PlayerJumpState(this);

        ChangeState(IdleState);
    }
}
