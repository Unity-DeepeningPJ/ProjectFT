using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
    }

    public override void HandleInput()
    {
        base.HandleInput();
        if (_stateMachine.MoveInput.x == 0)
        {
            _stateMachine.ChangeState(_stateMachine.IdleState);
        }
        else if (_stateMachine.Player.Controller.playerActions.Jump.triggered)
        {
            _stateMachine.ChangeState(_stateMachine.JumpState);
        }
        else if (_stateMachine.Player.Controller.playerActions.Dash.triggered)
        {
            _stateMachine.ChangeState(_stateMachine.DashState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        Move();
    }

    private void Move()
    {
        Vector2 moveDirection = new Vector2(_stateMachine.MoveInput.x, 0);
        float moveSpeed = player.PlayerState.speed;
        player.Rigidbody.velocity = new Vector2(moveDirection.x * moveSpeed, player.Rigidbody.velocity.y);
    }
}
