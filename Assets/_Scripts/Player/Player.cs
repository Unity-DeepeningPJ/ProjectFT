using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [field: SerializeField] public PlayerData Data { get; private set; }
    public PlayerState PlayerState { get; private set; }
    public PlayerCondition PlayerCondition { get; private set; }
    public PlayerController Controller { get; private set; }
    public PlayerStateMachine StateMachine { get; private set; }
    public Rigidbody2D Rigidbody { get; private set; }

    public bool isGrounded { get; set; } = true;

    private CurrencyManager currency;
    public CurrencyManager Currency => currency;

    private void Awake()
    {
        PlayerState = new PlayerState(Data);

        PlayerCondition = gameObject.AddComponent<PlayerCondition>();
        PlayerCondition.Initizlize(this, PlayerState.TotalHealth);

        Controller = GetComponent<PlayerController>();
        Rigidbody = GetComponent<Rigidbody2D>();
        StateMachine = new PlayerStateMachine(this);
        currency = GetComponent<CurrencyManager>();
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
        //몬스터 충돌 시 데미지 받는 로직
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        int enemyDamage = 0;

        if (collision.gameObject.layer == enemyLayer)
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            MeleeEnemy meleeEnemy = collision.gameObject.GetComponent<MeleeEnemy>();

            if (enemy != null)
                enemyDamage = 5;

            if (meleeEnemy != null)
                enemyDamage = meleeEnemy.attackDamage;

            PlayerTakePhysicalDamage(collision.collider, enemyDamage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //몬스터 공격에 맞았을 때 데미지 받는 로직
        int bulletLayer = LayerMask.NameToLayer("Bullet");
        int enemyDamage = 0;

        if (collision.gameObject.layer == bulletLayer)
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();

            if (bullet != null)
                enemyDamage = bullet.damageAmount;

            PlayerTakePhysicalDamage(collision, enemyDamage);
        }
    }

    private void PlayerTakePhysicalDamage(Collider2D collision, int damage)
    {
        Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
        float knockbackForce = 3f;

        PlayerCondition.TakePhysicalDamage(damage);
        PlayerCondition.ApplyKnockback(knockbackDirection, knockbackForce);
    }
}
