using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        player.Rigidbody.velocity = Vector2.zero;
    }

    public override void HandleInput()
    {
        base.HandleInput();
        if(_stateMachine.MoveInput.x != 0)
        {
            _stateMachine.ChangeState(_stateMachine.MoveState);
        }
        else if(_stateMachine.Player.Controller.playerActions.Jump.triggered)
        {
            _stateMachine.ChangeState(_stateMachine.JumpState);
        }
        else if (_stateMachine.Player.Controller.playerActions.Dash.triggered)
        {
            _stateMachine.ChangeState(_stateMachine.DashState);
        }
    }

    public override void Update()
    {
        base.Update();
    }
}
