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

        Debug.Log("Attack State");

        _attackRange = 3f;
        _attackDuration = 0.5f;
        _attackTimer = 0f;

        _attackFinished = false;

        CheckForHit();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void HandleInput()
    {
        base.HandleInput();

        if (_stateMachine.Player.Controller.playerActions.Jump.triggered)
        {
            _stateMachine.ChangeState(_stateMachine.JumpState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        Move();

        _attackTimer += Time.fixedDeltaTime;

        if (_attackTimer > _attackDuration)
        {
            if (!player.isGrounded)
                _stateMachine.ChangeState(_stateMachine.JumpState);
            else if (_stateMachine.MoveInput.x != 0)
                _stateMachine.ChangeState(_stateMachine.MoveState);
            else if (_stateMachine.MoveInput.x == 0)
                _stateMachine.ChangeState(_stateMachine.IdleState);
        }
    }

    private void CheckForHit()
    {
        Vector2 attackDirection = player.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        RaycastHit2D[] hits = Physics2D.RaycastAll(player.transform.position, attackDirection, _attackRange);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                Debug.Log("공격확인");
                var enemy = hit.collider.GetComponent<IDamageable>();

                if (enemy != null)
                {
                    Debug.Log("공격처리");
                    enemy.TakePhysicalDamage(player.PlayerState.TotalPower);
                }
            }
        }
    }
}
