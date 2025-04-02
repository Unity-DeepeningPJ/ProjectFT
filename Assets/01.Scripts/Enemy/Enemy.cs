using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BaseState, IDamageable
{
    public float moveDistance = 2f;
    public float moveSpeed = 2f;
    public float fireRate = 2f;
    public Transform firePoint;
    public LayerMask playerLayer;
    public float detectionRange = 5f;
    public int maxHealth = 200;
    public GameObject coinPrefab;

    private int currentHealth;
    private Vector2 startPosition;
    private bool movingRight = true;
    private float nextFireTime;
    private Transform playerTransform;
    private bool playerInRange = false;

    CharacterAnimation animi;
    SpriteRenderer sprite;

    Player player;

    public Enemy(int Power, int Defense, int health, float speed, float jumpPower) : base(Power, Defense, health, speed, jumpPower)
    {

    }

    void Start()
    {
        animi = GetComponent<CharacterAnimation>();
        sprite = GetComponent<SpriteRenderer>();
        player = GameManager.Instance.PlayerManager.player;

        // BaseState 초기화 (생성자를 사용하지 않는 경우)
        this.Power = Power;
        this.Defense = Defense;
        this.health = maxHealth;
        this.speed = speed;
        this.jumpPower = jumpPower;


        startPosition = transform.position;
        nextFireTime = Time.time;
        currentHealth = maxHealth;
    }

    void Update()
    {
        Move();
        FindPlayer();

        if (playerInRange && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Move()
    {
        if (movingRight)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            if (transform.position.x > startPosition.x + moveDistance)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
            if (transform.position.x < startPosition.x - moveDistance)
            {
                movingRight = true;
            }
        }
        sprite.flipX = movingRight;
    }

    void Shoot()
    {
        GameObject projectile = ObjectPool.Instance.GetPooledObject();
        if (projectile != null)
        {
            projectile.transform.position = firePoint.position;
            Vector2 direction = (playerTransform.position - firePoint.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            Bullet projectileScript = projectile.GetComponent<Bullet>();
            if (projectileScript != null)
            {
                projectileScript.SetDirection(direction);
            }

            projectile.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Projectile is null from Object Pool!");
        }
    }

    void FindPlayer()
    {
        GameObject player = FindClosestObjectWithLayerInRange(transform.position, playerLayer, detectionRange);

        if (player != null)
        {
            animi.OnAttack(true);
            sprite.flipX = player.transform.position.x - transform.position.x > 0;
            playerTransform = player.transform;
            playerInRange = true;
        }
        else
        {
            animi.OnAttack(false);
            playerTransform = null;
            playerInRange = false;
        }
    }

    GameObject FindClosestObjectWithLayerInRange(Vector3 position, LayerMask layer, float range)
    {
        var goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        GameObject closest = null;
        var distance = Mathf.Infinity;
        Vector3 pos = position;
        foreach (var go in goArray)
        {
            if ((layer.value & (1 << go.layer)) != 0)
            {
                Vector3 diff = go.transform.position - pos;
                float curDistance = diff.sqrMagnitude;

                if (curDistance < distance && curDistance <= range * range)
                {
                    closest = go;
                    distance = curDistance;
                }
            }
        }
        return closest;
    }

    public void TakePhysicalDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Instantiate(coinPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);

        if (player != null)
        {
            player.PlayerState.AddExp(30); // 경험치 추가
            //Debug.Log("Player Exp: " + player.PlayerState.CurrentExp);
            //Debug.Log("Player Level: " + player.PlayerState.Level);
        }
    }
}
