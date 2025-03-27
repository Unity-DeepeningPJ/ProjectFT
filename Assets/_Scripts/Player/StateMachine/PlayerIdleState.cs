using UnityEngine;

public class PlayerIdleState : IState
{
    private PlayerStateMachine _stateMachine;

    public PlayerIdleState(PlayerStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void Enter() { }

    public void Exit() { }

    public void HandleInput()
    {
        if(_stateMachine.Player.Controller.GetMoveInput() != 0)
        {
            _stateMachine.ChangeState(_stateMachine.MoveState);
        }
    }

    public void Update() { }

    public void PhysicsUpdate()
    {
    }
}
