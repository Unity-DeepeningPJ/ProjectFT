using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MeleeEnemy
{
    [Header("Health")]
    public int maxHealth = 200; // �ִ� ü��
    private int currentHealth; // ���� ü��

    [Header("Special Attack")]
    public float specialAttackInterval = 10f; // Ư�� ���� ����
    public float specialAttackDashSpeed = 7f; // Ư�� ���� ���� �ӵ�
    public float specialAttackDashDistance = 3f; // Ư�� ���� ���� �Ÿ�
    public int projectileCount = 5; // ����ü �߻� ����
    public float projectileSpreadAngle = 30f; // ����ü Ȯ�� ����
    public GameObject projectilePrefab; // ����ü ������

    [Header("References")]
    public Transform firePoint; // ����ü �߻� ��ġ

    
    private float nextSpecialAttackTime;
    private bool isSpecialAttacking = false; // Ư�� ���� ������ ����

    public Boss(int Power, int Defense, int health, float speed, float jumpPower) : base(Power, Defense, health, speed, jumpPower)
    {
    }

    void Start()
    {
        currentHealth = maxHealth;
        nextSpecialAttackTime = Time.time + specialAttackInterval;
        startPosition = transform.position;
        nextAttackTime = Time.time;

        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D ������Ʈ ��������
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found!");
        }
        //Move(); // �ʱ� �̵� ���� (����)
        transform.localScale = new Vector3(1, 1, 1); // ���� ũ�� Ű���
        moveSpeed = 0f; // ���� �̵� �ӵ� 0���� ����
        chaseSpeed = 0f; // ���� �߰� �ӵ� 0���� ����

        // �÷��̾� Transform ã��
        GameObject player = FindClosestObjectWithLayer(transform.position, playerLayer);
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player not found in layer!");
        }
    }

    void Update()
    {
        Debug.Log("���� ü��" + currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }

        //FindPlayer(); // �÷��̾� ã�� ���� (�׻� �÷��̾� ��ġ�� ����)

        Debug.Log("Is Attacking: " + isAttacking); // isAttacking �� Ȯ��
        Debug.Log("Is Special Attacking: " + isSpecialAttacking); // isSpecialAttacking �� Ȯ��

        HandleState();
    }

    void HandleState()
    {
        if (isSpecialAttacking)
        {
            // Ư�� ���� ��
            return;
        }

        if (isIdle)
        {
            HandleIdleState();
        }
        else if (!isAttacking)
        {
            HandleMovementState();
        }
        else
        {
            Dash(); // ����
        }

        // Ư�� ���� ��Ÿ�� Ȯ�� �� ����
        if (Time.time >= nextSpecialAttackTime && !isSpecialAttacking && playerTransform != null) // �÷��̾ �־����
        {
            SpecialAttack();
            nextSpecialAttackTime = Time.time + specialAttackInterval;
        }
    }

    void HandleIdleState()
    {
        Debug.Log("idleTime : " + idleTime);

        if (Time.time >= stopTime)
        {
            isIdle = false;
            StopAttack();
            Debug.Log("���̵� ��");
        }


    }

    void HandleMovementState()
    {
        if (playerTransform != null)
        {
            StartAttack();
        }
    }

    void Move()
    {
        // �¿� �̵� ���� (�� �̻� ������� ����)
    }

    void ChasePlayer()
    {
        isChasing = true;
        // �÷��̾� �������� �̵�
        if (playerTransform != null) // playerTransform�� null�� �ƴ��� Ȯ��
        {
            Vector2 direction = new Vector2(playerTransform.position.x - transform.position.x, 0).normalized;
            rb.velocity = new Vector2(direction.x * chaseSpeed, rb.velocity.y); // Rigidbody2D�� ����Ͽ� �̵�
            Debug.Log("Chasing Velocity: " + rb.velocity); // �߰� �ӵ� Ȯ��
        }
        else
        {
            Debug.LogError("playerTransform is null in ChasePlayer()!");
            rb.velocity = Vector2.zero;
            isChasing = false; // playerTransform�� null�̸� �߰� �ߴ�
        }
    }

    void StartAttack()
    {
        if (Time.time >= nextAttackTime && !isSpecialAttacking && playerTransform != null)
        {
            isAttacking = true;
            isChasing = true;
            Debug.Log("Start Attack!");

            // ���� ��ǥ ��ġ ����
            dashTarget = playerTransform.position;
            dashStartTime = Time.time;

            Vector2 direction = new Vector2(dashTarget.x - transform.position.x, 0).normalized;
            rb.velocity = direction * chaseSpeed;
            nextAttackTime = Time.time + 1f / attackRate; // ���� �� ��Ÿ�� ����
        }
    }

    void Dash()
    {
        if (isChasing)
        {
            float timeSinceStarted = Time.time - dashStartTime;
            float percentageComplete = timeSinceStarted / dashDuration;

            if (percentageComplete >= 1)
            {
                StopAttack();
            }

        }
    }

    void StopAttack()
    {
        isAttacking = false;
        isChasing = false;
        isIdle = true;
        stopTime = Time.time + idleTime;
        rb.velocity = Vector2.zero; // ����
        Debug.Log("Stop Attack!");
    }

    void SpecialAttack()
    {
        // Ư�� ���� ���� ����
        Debug.Log("Boss Special Attack!");

        isSpecialAttacking = true; // Ư�� ���� �� ���·� ����

        // 1. �÷��̾� �������� ª�� ����
        StartCoroutine(SpecialAttackDash());
    }

    System.Collections.IEnumerator SpecialAttackDash()
    {
        // ����
        if (playerTransform != null)
        {
            Vector2 direction = new Vector2(playerTransform.position.x - transform.position.x, 0).normalized;
            rb.velocity = direction * specialAttackDashSpeed;

            float timer = 0;
            while (timer < dashDuration)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            rb.velocity = Vector2.zero;

            // ����ü ���� �� �߻�
            StartCoroutine(SpecialAttackShoot());
        }
        isSpecialAttacking = false;
    }

    System.Collections.IEnumerator SpecialAttackShoot()
    {
        // ����ü ���� �� �߻�

        // �÷��̾�� ���� ������ ���� ���
        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
        float angleToPlayer = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

        float startAngle = angleToPlayer - projectileSpreadAngle / 2f;
        float angleStep = projectileSpreadAngle / (projectileCount - 1);

        for (int i = 0; i < projectileCount; i++)
        {
            float angle = startAngle + angleStep * i;
            Vector2 direction = Quaternion.AngleAxis(angle, Vector3.forward) * Vector2.right;

            GameObject projectile = ObjectPool.Instance.GetPooledObject();

            if (projectile != null)
            {
                // ����ü ���� ��ġ ����
                Vector3 spawnPosition = firePoint.position + (Vector3)direction * 1f; // firePoint���� ���� �� �ٱ������� ����
                projectile.transform.position = spawnPosition;
                projectile.transform.rotation = Quaternion.identity;

                Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();

                if (projectileRb != null)
                {
                    projectileRb.velocity = direction * 5f; // ����ü �ӵ� ����
                    projectile.SetActive(true);
                }
                else
                    Debug.LogError("����ü�� rigidbody�� �����ϴ�");
            }
            else
                Debug.LogError("ObjectPool�� ����ü�� �����ϴ�");

            Debug.Log("Projectile �߻�!");
            yield return null; // ����ü �߻� ����
        }

        isSpecialAttacking = false; // Ư�� ���� ����
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Boss ������ ����! ���� ü��: " + currentHealth);
    }

    void Die()
    {
        Debug.Log("Boss Die!");
        Destroy(gameObject); // �Ǵ� ������Ʈ Ǯ�� ��ȯ
    }

    GameObject FindClosestObjectWithLayer(Vector3 position, LayerMask layer)
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
                if (curDistance < distance)
                {
                    closest = go;
                    distance = curDistance;
                }
            }
        }
        return closest;
    }
}
