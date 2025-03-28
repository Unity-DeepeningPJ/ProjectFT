using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        //점프
        if (player.isGrounded)
        {
            player.Rigidbody.velocity = new Vector2(player.Rigidbody.velocity.x, player.PlayerState.jumpPower);
            player.isGrounded = false;
        }
    }

    public override void HandleInput()
    {
        base.HandleInput();

        if (_stateMachine.Player.Controller.playerActions.Dash.triggered)
        {
            _stateMachine.ChangeState(_stateMachine.DashState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        //점프가 끝나면 상태 변경
        if (!player.isGrounded && _stateMachine.Player.Rigidbody.velocity.y <= 0)
        {
            player.isGrounded = true;

            if (_stateMachine.MoveInput.x != 0)
                _stateMachine.ChangeState(_stateMachine.MoveState);
            else
                _stateMachine.ChangeState(_stateMachine.IdleState);
        }
    }
}
