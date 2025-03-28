using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    private Vector2 _dashTargetPosition;
    private float _dashDirection;
    private float _gravityScale;
    private float _fixedYPosition;

    public PlayerDashState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        //대쉬 중 중력, 점프 영향 받지 않도록 함
        _gravityScale = player.Rigidbody.gravityScale;
        player.Rigidbody.gravityScale = 0f;

        player.Rigidbody.velocity = new Vector2(player.Rigidbody.velocity.x, 0f);

        //대쉬 방향
        if (_stateMachine.MoveInput.x != 0)
            _dashDirection = _stateMachine.MoveInput.x;
        else
            _dashDirection = player.transform.localScale.x;

        //현재 y 값 고정
        _fixedYPosition = player.Rigidbody.position.y;

        //대쉬 위치 계산
        _dashTargetPosition = new Vector2(player.transform.position.x + (_dashDirection * player.PlayerState.DashDistance), _fixedYPosition);

        //대쉬 중 무적
        player.isInvincible = true;
    }

    public override void Exit()
    {
        base.Exit();

        player.isInvincible = false;

        //원래 중력 값 복원
        player.Rigidbody.gravityScale = _gravityScale;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (player.isInvincible)
        {
            //대쉬 이동 처리
            player.transform.position = Vector2.MoveTowards(
                player.transform.position,
                _dashTargetPosition,
                player.PlayerState.DashSpeed * Time.fixedDeltaTime);

            //대쉬 종료
            if (Vector2.Distance(player.transform.position, _dashTargetPosition) < 0.1f)
            {
                player.isInvincible = false;

                //점프 중이었다면 점프 상태로 돌아감
                if (!player.isGrounded)
                    _stateMachine.ChangeState(_stateMachine.JumpState);
                else
                    _stateMachine.ChangeState(_stateMachine.IdleState);
            }
        }
    }
}
