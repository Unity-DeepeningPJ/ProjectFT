using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        
        Debug.Log("Move State");
        player.animi.OnMove(true);
    }

    public override void Exit()
    {
        base.Exit();
        player.animi.OnMove(false);
        if (player.Controller.canMove && player.isGrounded)
            player.Rigidbody.velocity = Vector2.zero;
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
}
