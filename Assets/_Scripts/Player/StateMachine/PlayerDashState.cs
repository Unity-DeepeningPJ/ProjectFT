using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    private Vector2 _dashTargetPosition;
    private float _dashSpeed = 10f;
    private float _dashDirection;

    public PlayerDashState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        if (_stateMachine.MoveInput.x != 0)
            _dashDirection = _stateMachine.MoveInput.x;
        else
            _dashDirection = _stateMachine.Player.transform.localScale.x;

        _dashTargetPosition = (Vector2)_stateMachine.Player.transform.position + new Vector2(_dashDirection * _stateMachine.Player.PlayerState.DashDistance, 0);

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

        if (player.isInvincible)
        {
            _stateMachine.Player.transform.position = Vector2.MoveTowards(
                _stateMachine.Player.transform.position,
                _dashTargetPosition,
                _dashSpeed * Time.deltaTime);

            if (Vector2.Distance(_stateMachine.Player.transform.position, _dashTargetPosition) < 0.1f)
            {
                player.isInvincible = false;
                _stateMachine.ChangeState(_stateMachine.IdleState);
            }
        }
    }
}
