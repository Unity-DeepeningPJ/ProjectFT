using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    private Vector2 _dashDirection;
    private float _dashDistance;

    public PlayerDashState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        _dashDirection = _stateMachine.Player.transform.right * _stateMachine.MoveInput.x;
        _dashDistance = _stateMachine.Player.PlayerState.DashDistance;

        _stateMachine.Player.Rigidbody.velocity = _dashDirection * 10f;
        player.isInvincible = true;
    }

    public override void Exit()
    {
        base.Exit();
        player.isInvincible = false;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        Dash();
    }

    private void Dash()
    {
        if (player.isInvincible)
        {
            float ditanceMoved = Vector2.Distance(_stateMachine.Player.transform.position, _dashDirection);
            if (ditanceMoved >= _stateMachine.Player.PlayerState.DashDistance)
            {
                player.isInvincible = false;
                _stateMachine.ChangeState(_stateMachine.IdleState);
            }
        }
    }
}
