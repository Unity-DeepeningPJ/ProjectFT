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

        player.animi.OnAttack(true);
        // Debug.Log("Attack State");

        _attackRange = 5f;
        _attackDuration = 0.5f;
        _attackTimer = 0f;

        _attackFinished = false;

        CheckForHit();
    }

    public override void Exit()
    {
        player.animi.OnAttack(false);
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
        Vector2 attackDirection = player.transform.localScale.x > 0 ? Vector2.left : Vector2.right;
        RaycastHit2D[] hits = Physics2D.RaycastAll(player.transform.position, attackDirection, _attackRange);

        int damage = player.PlayerState.TotalPower;

        foreach (RaycastHit2D hit in hits)
        {
            AudioManager.Instance.PlaySFX("Attack");
            if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                var enemy = hit.collider.GetComponent<IDamageable>();

                if (enemy != null)
                {
                    //치명타 확률 적용
                    bool isCritical = Random.Range(0f, 100f) < player.PlayerState.TotalCriticalChance;
                    if (isCritical) damage *= 2;
                    
                    enemy.TakePhysicalDamage(damage);
                    //Debug.Log("적에게 " + damage + "의 피해를 입혔습니다.");
                }
            }
        }
    }
}
