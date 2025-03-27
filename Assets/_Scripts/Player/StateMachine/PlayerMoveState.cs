using UnityEngine;

public class PlayerMoveState : IState
{
    private PlayerStateMachine _stateMachine;
    private Rigidbody2D _rigidBody;
    private float moveSpeed;

    public PlayerMoveState(PlayerStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
        _rigidBody = stateMachine.Player.GetComponent<Rigidbody2D>();
        moveSpeed = stateMachine.Player.Data.baseSpeed;
    }

    public void Enter() { }

    public void Exit() { }

    public void HandleInput()
    {
        float moveInput = _stateMachine.Player.Controller.GetMoveInput();

        if (moveInput == 0)
        {
            _stateMachine.ChangeState(_stateMachine.IdleState);
        }
    }

    public void Update() { }

    public void PhysicsUpdate()
    {
        float moveInput = _stateMachine.Player.Controller.GetMoveInput();

        Vector3 newPosition = _rigidBody.position;
        newPosition.x += moveInput * moveSpeed * Time.fixedDeltaTime;

        _rigidBody.position = newPosition;
    }
}
