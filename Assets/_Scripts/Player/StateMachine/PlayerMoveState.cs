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

        //각 입력에 맞는 상태로 변환
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
        else if (_stateMachine.Player.Controller.playerActions.Attack.triggered)
        {
            _stateMachine.ChangeState(_stateMachine.AttackState);
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

        //플레이어 방향 전환
        if(moveDirection.x != 0)
        {
            player.transform.localScale = new Vector3(moveDirection.x, 1, 1);
        }
    }
}
