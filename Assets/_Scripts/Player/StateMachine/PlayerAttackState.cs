using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    private float _attackRange;
    private float _attackDuration;
    private float _attackTimer;
    public bool _attackFinished;

    public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        _attackRange = 3f;
        _attackDuration = 0.5f;
        _attackTimer = 0f;

        _attackFinished = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void HandleInput()
    {
        base.HandleInput();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (!_attackFinished)
        {
            _attackTimer += Time.fixedDeltaTime;

            if (_attackTimer > _attackDuration)
            {
                _attackFinished = true;
                _stateMachine.ChangeState(_stateMachine.IdleState);
            }
        }

        CheckForHit();
    }

    private void CheckForHit()
    {
        Vector2 attackDirection = _stateMachine.MoveInput;
        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, attackDirection, _attackRange);

        //TODO:hit.collider.CompareTag("") 확인 및 데미지 로직
        //if(hit.collider != null && hit.collider.CompareTag(""))
        //{
        //}
    }
}
