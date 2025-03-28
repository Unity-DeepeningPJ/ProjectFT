using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [field: SerializeField] public PlayerData Data { get; private set; }
    public PlayerState PlayerState { get; private set; }
    public PlayerCondition PlayerCondition { get; private set; }
    public PlayerController Controller { get; private set; }
    public PlayerStateMachine StateMachine { get; private set; }
    public Rigidbody2D Rigidbody { get; private set; }

    private Collider2D _playerCollider;
    private Collider2D _enemyCollider;

    public bool isGrounded { get; set; } = true;

    private CurrencyManager currency;
    public CurrencyManager Currency => currency;

    private void Awake()
    {
        PlayerState = new PlayerState(Data);

        PlayerCondition = new PlayerCondition(this, PlayerState.TotalHealth);
        Controller = GetComponent<PlayerController>();
        Rigidbody = GetComponent<Rigidbody2D>();
        StateMachine = new PlayerStateMachine(this);
        currency =GetComponent<CurrencyManager>();
        _playerCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        StateMachine.HandleInput();
        StateMachine.Update();
    }

    private void FixedUpdate()
    {
        StateMachine.PhysicsUpdate();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            _enemyCollider = collision.collider;

            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
            float knockbackForce = 3f;

            PlayerCondition.SetCollider(_enemyCollider, _playerCollider);
            PlayerCondition.TakePhysicalDamage(5);
            PlayerCondition.ApplyKnockback(knockbackDirection, knockbackForce);
        }
    }
}
