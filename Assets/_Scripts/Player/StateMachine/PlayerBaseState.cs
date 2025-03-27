using UnityEngine;

public class PlayerBaseState : IState
{
    public PlayerStateMachine _stateMachine { get; private set; }
    protected Player player;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
        player = stateMachine.Player;
    }

    public virtual void Enter() { }

    public virtual void Exit() { }

    public virtual void HandleInput()
    {
        _stateMachine.MoveInput = player.Controller.playerActions.Move.ReadValue<Vector2>();
    }

    public virtual void PhysicsUpdate() { }

    public virtual void Update() { }
}
