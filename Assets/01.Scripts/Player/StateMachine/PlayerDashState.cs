using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    private Vector2 _dashTargetPosition;
    private float _dashDirection;
    private float _gravityScale;
    private float _fixedYPosition;

    private float _dashDistance;
    private float _dashSpeed;
    private float _dashTime;

    public PlayerDashState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        //Debug.Log("Dash State");

        _dashDistance = player.PlayerState.DashDistance;
        _dashSpeed = player.PlayerState.DashSpeed;
        _dashTime = _dashDistance / _dashSpeed;

        //대쉬 중 중력, 점프 영향 받지 않도록 함
        _gravityScale = player.Rigidbody.gravityScale;
        player.Rigidbody.gravityScale = 0f;
        player.Rigidbody.velocity = new Vector2(player.Rigidbody.velocity.x, 0f);

        //대쉬 방향
        if (_stateMachine.MoveInput.x != 0)
            _dashDirection = _stateMachine.MoveInput.x;
        else
            _dashDirection = -player.transform.localScale.x;

        //현재 y 값 고정
        _fixedYPosition = player.Rigidbody.position.y;

        //대쉬 거리 안 장애물 체크
        int ignoreLayers = LayerMask.GetMask("Player", "Enemy", "Camera");
        int mapLayers = ~ignoreLayers;

        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, Vector2.right * _dashDirection, _dashDistance, mapLayers);

        //대쉬 위치 계산
        if (hit.collider != null)
        {
            _dashTargetPosition = new Vector2(hit.point.x - (_dashDirection * 0.5f), _fixedYPosition);
        }
        else
        {
            _dashTargetPosition = new Vector2(player.transform.position.x + (_dashDirection * player.PlayerState.DashDistance), _fixedYPosition);
        }

        isDash = true;

        //대쉬 중 무적
        player.StartCoroutine(player.PlayerCondition.InvincibilityFrames(_dashTime));
    }

    public override void Exit()
    {
        base.Exit();

        player.PlayerCondition.IsInvincible = false;

        //원래 중력 값 복원
        player.Rigidbody.gravityScale = _gravityScale;
        player.Rigidbody.velocity = Vector2.zero;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (isDash)
        {
            player.Rigidbody.velocity = new Vector2(_dashDirection * _dashSpeed, player.Rigidbody.velocity.y);

            //대쉬 이동 처리
            player.transform.position = Vector2.MoveTowards(
                player.transform.position,
                _dashTargetPosition,
                player.PlayerState.DashSpeed * Time.fixedDeltaTime);

            //대쉬 종료
            if (Vector2.Distance(player.transform.position, _dashTargetPosition) <= 0.1f)
            {
                isDash = false;

                //점프 중이었다면 점프 상태로 돌아감
                if (!player.isGrounded)
                    _stateMachine.ChangeState(_stateMachine.JumpState);
                else
                    _stateMachine.ChangeState(_stateMachine.IdleState);
            }
        }
    }
}
