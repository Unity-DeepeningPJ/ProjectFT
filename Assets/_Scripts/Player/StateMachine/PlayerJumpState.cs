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
            player.Rigidbody.AddForce(Vector2.up * player.PlayerState.jumpPower, ForceMode2D.Impulse);
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
        else if (_stateMachine.Player.Controller.playerActions.Attack.triggered)
        {
            _stateMachine.ChangeState(_stateMachine.AttackState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        Move();

        //점프가 끝나면 상태 변경
        if (!player.isGrounded && Mathf.Abs(player.Rigidbody.velocity.y) <= 0.1f)
        {
            player.isGrounded = true;

            if (_stateMachine.MoveInput.x != 0)
                _stateMachine.ChangeState(_stateMachine.MoveState);
            else
                _stateMachine.ChangeState(_stateMachine.IdleState);
        }
    }
}
