using UnityEngine;
using UnityEngine.UI;

public class PlayerBaseState : IState
{
    public PlayerStateMachine _stateMachine { get; private set; }
    protected Player player;
    protected bool isDash = false;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
        player = stateMachine.Player;
    }

    public virtual void Enter() { }

    public virtual void Exit() { }

    public virtual void HandleInput()
    {
        _stateMachine.MoveInput = player.Controller.playerActions.Move.ReadValue<Vector2>();
    }

    public virtual void PhysicsUpdate() { }

    public virtual void Update() { }

    public void Move()
    {
        if (player.Controller.canMove == false) return;

        Vector2 moveDirection = new Vector2(_stateMachine.MoveInput.x, 0);
        float moveSpeed = player.PlayerState.speed;

        if (!isDash)
        {
            player.Rigidbody.velocity = new Vector2(moveDirection.x * moveSpeed, player.Rigidbody.velocity.y);

            //플레이어 방향 전환
            if (moveDirection.x != 0)
            {
                player.transform.localScale = new Vector3(-moveDirection.x, 1, 1);
            }
        }
    }
}
