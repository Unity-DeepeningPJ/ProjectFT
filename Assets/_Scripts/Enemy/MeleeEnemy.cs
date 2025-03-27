using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MeleeEnemy : BaseState
{
    public float moveDistance = 2f; // �̵� �Ÿ�
    public float moveSpeed = 2f; // �̵� �ӵ�
    public float chaseSpeed = 5f; // �߰� �ӵ�
    public float attackDamage = 10f; // ���� ������
    public float attackRate = 1f; // ���� �ӵ� (�ʴ� ���� Ƚ��)
    public LayerMask playerLayer; // �÷��̾� ���̾�
    public float detectionRange = 5f; // �÷��̾� �ν� ����
    public float attackRange = 1f; // ���� ����
    public float dashDistance = 3f; // ���� �Ÿ�
    public float idleTime = 1f; // �����ִ� �ð�

    private Vector2 startPosition;
    private bool movingRight = true;
    private float nextAttackTime;
    private Transform playerTransform; // �÷��̾��� Transform
    private bool playerInRange = false; // �÷��̾ ���� ���� �ִ��� ����
    private bool isChasing = false; // �߰� ������ ����
    private bool isAttacking = false; // ���� ������ ����
    private Vector2 dashTarget; // ���� ��ǥ ��ġ
    private float dashStartTime; // ���� ���� �ð�
    private float stopTime; // ���� �ð�
    public float dashDuration = 0.5f; // ���� ���� �ð�
    private bool isIdle = false; // �����ִ��� ����

    private Rigidbody2D rb; // Rigidbody2D ������Ʈ

    public MeleeEnemy(int Power, int Defense, int health, float speed, float jumpPower) : base(Power, Defense, health, speed, jumpPower)
    {
    }

    void Start()
    {
        startPosition = transform.position;
        nextAttackTime = Time.time;

        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D ������Ʈ ��������
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found!");
        }
        Move(); // �ʱ� �̵� ����
    }

    void Update()
    {
        FindPlayer();

        Debug.Log("Player In Range: " + playerInRange); // playerInRange �� Ȯ��
        Debug.Log("Is Attacking: " + isAttacking); // isAttacking �� Ȯ��
        Debug.Log("Is Idle: " + isIdle); // isIdle �� Ȯ��

        if (isIdle)
        {
            Debug.Log("Idle Time: " + (Time.time - stopTime)); // �����ִ� �ð� Ȯ��
            if (Time.time - stopTime >= idleTime)
            {
                isIdle = false;
                Debug.Log("Moving after Idle"); // Move ȣ�� Ȯ��

                FindPlayer();
                if (playerInRange && playerTransform != null)
                {
                    StartAttack();
                }
                else
                {
                    Move();
                }
            }
        }
        else if (!isAttacking)
        {
            if (playerInRange && playerTransform != null) // �÷��̾ ���� ���� �ְ� playerTransform�� null�� �ƴ� ���� �߰�
            {
                StartAttack(); // ���� ����
            }
            else
            {
                Move(); // �⺻ �̵�
                Debug.Log("Moving"); // Move �Լ� ȣ�� Ȯ��
            }
        }
        else
        {
            Dash(); // ����
        }
    }

    void Move()
    {
        // �¿� �̵� ���� (���� �ڵ�� ����)
        if (movingRight)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            if (transform.position.x > startPosition.x + moveDistance)
            {
                movingRight = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            if (transform.position.x < startPosition.x - moveDistance)
            {
                movingRight = true;
            }
        }
        Debug.Log("Current Velocity: " + rb.velocity); // ���� �ӵ� Ȯ��
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
        if (Time.time >= nextAttackTime)
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
        stopTime = Time.time;
        rb.velocity = Vector2.zero; // ����
        Debug.Log("Stop Attack!");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // �÷��̾�� �浹 �� ������ �ֱ�
        if (isAttacking && ((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            Debug.Log("Attack!");
            // �������� �ִ� ���� ����
            // ��: collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
        }

        StopAttack();
    }

    void FindPlayer()
    {
        GameObject player = FindClosestObjectWithLayerInRange(transform.position, playerLayer, detectionRange);

        if (player != null)
        {
            playerTransform = player.transform;
            playerInRange = true; // �÷��̾ ���� ���� ����
            Debug.Log("Player Found: " + player.name); // �÷��̾� ã�� Ȯ��
            Debug.Log("Player Transform: " + playerTransform); // �÷��̾� Transform Ȯ��
        }
        else
        {
            playerTransform = null;
            playerInRange = false; // �÷��̾ ���� �ۿ� ����
            isChasing = false;
            //rb.velocity = Vector2.zero; // �÷��̾ ã�� ���ϸ� �ӵ� �ʱ�ȭ (�� �κ��� ����)
            Debug.Log("Player Not Found"); // �÷��̾� ã�� ���� Ȯ��
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

                if (curDistance < distance && curDistance <= range * range) // ���� ���� �ִ��� Ȯ��
                {
                    closest = go;
                    distance = curDistance;
                    Debug.Log("Found object in layer: " + go.name + ", distance: " + curDistance); // ���̾ ���� ������Ʈ ã�� Ȯ��
                }
            }
        }
        return closest;
    }
}
