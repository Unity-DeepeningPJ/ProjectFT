﻿using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        player.Rigidbody.velocity = new Vector2(player.Rigidbody.velocity.x, player.PlayerState.jumpPower);
        player.isGrounded = false;
    }

    public override void HandleInput()
    {
        base.HandleInput();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (!player.isGrounded && _stateMachine.Player.Rigidbody.velocity.y <= 0)
        {
            if (_stateMachine.MoveInput.x != 0)
                _stateMachine.ChangeState(_stateMachine.MoveState);
            else
                _stateMachine.ChangeState(_stateMachine.IdleState);
        }
    }
}
