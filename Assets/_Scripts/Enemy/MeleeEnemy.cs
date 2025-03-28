using UnityEngine;



public class MeleeEnemy : BaseState
{
    [Header("Movement")]
    public float moveDistance = 2f; // 이동 거리
    public float moveSpeed = 2f; // 이동 속도
    public float chaseSpeed = 5f; // 추격 속도

    [Header("Attack")]
    public float attackDamage = 10f; // 공격 데미지
    public float attackRate = 1f; // 공격 속도 (초당 공격 횟수)
    public float dashDistance = 3f; // 돌진 거리
    public float dashDuration = 0.5f; // 돌진 지속 시간

    [Header("Detection")]
    public LayerMask playerLayer; // 플레이어 레이어
    public float detectionRange = 5f; // 플레이어 인식 범위
    public float attackRange = 1f; // 공격 범위

    [Header("Idle")]
    public float idleTime = 1f; // 멈춰있는 시간

    protected Vector2 startPosition;
    protected bool movingRight = true;
    protected float nextAttackTime;
    protected Transform playerTransform; // 플레이어의 Transform
    protected bool playerInRange = false; // 플레이어가 범위 내에 있는지 여부
    protected bool isChasing = false; // 추격 중인지 여부
    protected bool isAttacking = false; // 공격 중인지 여부
    protected Vector2 dashTarget; // 돌진 목표 위치
    protected float dashStartTime; // 돌진 시작 시간
    protected float stopTime; // 멈춘 시간
    protected bool isIdle = false; // 멈춰있는지 여부

    protected Rigidbody2D rb; // Rigidbody2D 컴포넌트

    public MeleeEnemy(int Power, int Defense, int health, float speed, float jumpPower) : base(Power, Defense, health, speed, jumpPower)
    {
    }

    void Start()
    {
        startPosition = transform.position;
        nextAttackTime = Time.time;

        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D 컴포넌트 가져오기
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found!");
        }
        Move(); // 초기 이동 시작
    }

    void Update()
    {
        FindPlayer();



        if (isIdle)
        {

            if (Time.time - stopTime >= idleTime)
            {
                isIdle = false;

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
            if (playerInRange && playerTransform != null) // 플레이어가 범위 내에 있고 playerTransform이 null이 아닐 때만 추격
            {
                StartAttack(); // 공격 시작
            }
            else
            {
                Move(); // 기본 이동

            }
        }
        else
        {
            Dash(); // 돌진
        }
    }

    public void Move()
    {
        // 좌우 이동 로직 (기존 코드와 동일)
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

    }

    void ChasePlayer()
    {
        isChasing = true;
        // 플레이어 방향으로 이동
        if (playerTransform != null) // playerTransform이 null이 아닌지 확인
        {
            Vector2 direction = new Vector2(playerTransform.position.x - transform.position.x, 0).normalized;
            rb.velocity = new Vector2(direction.x * chaseSpeed, rb.velocity.y); // Rigidbody2D를 사용하여 이동
            Debug.Log("Chasing Velocity: " + rb.velocity); // 추격 속도 확인
        }
        else
        {
            Debug.LogError("playerTransform is null in ChasePlayer()!");
            rb.velocity = Vector2.zero;
            isChasing = false; // playerTransform이 null이면 추격 중단
        }
    }

    public void StartAttack()
    {
        if (Time.time >= nextAttackTime)
        {
            isAttacking = true;
            isChasing = true;


            // 돌진 목표 위치 설정
            dashTarget = playerTransform.position;
            dashStartTime = Time.time;

            Vector2 direction = new Vector2(dashTarget.x - transform.position.x, 0).normalized;
            rb.velocity = direction * chaseSpeed;
            nextAttackTime = Time.time + 1f / attackRate; // 공격 후 쿨타임 적용
        }
    }

    public void Dash()
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

    public void StopAttack()
    {
        isAttacking = false;
        isChasing = false;
        isIdle = true;
        stopTime = Time.time;
        rb.velocity = Vector2.zero; // 멈춤

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 플레이어와 충돌 시 데미지 주기
        if (isAttacking && ((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            Debug.Log("Attack!");
            // 데미지를 주는 로직 구현

        }

        StopAttack();
    }

    public void FindPlayer()
    {
        GameObject player = FindClosestObjectWithLayerInRange(transform.position, playerLayer, detectionRange);

        if (player != null)
        {
            playerTransform = player.transform;
            playerInRange = true; // 플레이어가 범위 내에 있음

        }
        else
        {
            playerTransform = null;
            playerInRange = false; // 플레이어가 범위 밖에 있음
            isChasing = false;


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

                if (curDistance < distance && curDistance <= range * range) // 범위 내에 있는지 확인
                {
                    closest = go;
                    distance = curDistance;

                }
            }
        }
        return closest;
    }
}
