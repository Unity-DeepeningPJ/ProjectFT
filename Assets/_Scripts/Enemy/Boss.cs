using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MeleeEnemy
{
    [Header("Health")]
    public int maxHealth = 200; // 최대 체력
    private int currentHealth; // 현재 체력

    [Header("Special Attack")]
    public float specialAttackInterval = 10f; // 특수 공격 간격
    public float specialAttackDashSpeed = 7f; // 특수 공격 돌진 속도
    public float specialAttackDashDistance = 3f; // 특수 공격 돌진 거리
    public int projectileCount = 5; // 투사체 발사 개수
    public float projectileSpreadAngle = 30f; // 투사체 확산 각도
    public GameObject projectilePrefab; // 투사체 프리팹

    [Header("References")]
    public Transform firePoint; // 투사체 발사 위치

    
    private float nextSpecialAttackTime;
    private bool isSpecialAttacking = false; // 특수 공격 중인지 여부

    public Boss(int Power, int Defense, int health, float speed, float jumpPower) : base(Power, Defense, health, speed, jumpPower)
    {
    }

    void Start()
    {
        currentHealth = maxHealth;
        nextSpecialAttackTime = Time.time + specialAttackInterval;
        startPosition = transform.position;
        nextAttackTime = Time.time;

        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D 컴포넌트 가져오기
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found!");
        }
        //Move(); // 초기 이동 시작 (제거)
        transform.localScale = new Vector3(1, 1, 1); // 보스 크기 키우기
        moveSpeed = 0f; // 보스 이동 속도 0으로 설정
        chaseSpeed = 0f; // 보스 추격 속도 0으로 설정

        // 플레이어 Transform 찾기
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
        Debug.Log("남은 체력" + currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }

        //FindPlayer(); // 플레이어 찾기 제거 (항상 플레이어 위치에 공격)

        Debug.Log("Is Attacking: " + isAttacking); // isAttacking 값 확인
        Debug.Log("Is Special Attacking: " + isSpecialAttacking); // isSpecialAttacking 값 확인

        HandleState();
    }

    void HandleState()
    {
        if (isSpecialAttacking)
        {
            // 특수 공격 중
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
            Dash(); // 돌진
        }

        // 특수 공격 쿨타임 확인 및 실행
        if (Time.time >= nextSpecialAttackTime && !isSpecialAttacking && playerTransform != null) // 플레이어가 있어야함
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
            Debug.Log("아이들 끝");
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
        // 좌우 이동 로직 (더 이상 사용하지 않음)
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

    void StartAttack()
    {
        if (Time.time >= nextAttackTime && !isSpecialAttacking && playerTransform != null)
        {
            isAttacking = true;
            isChasing = true;
            Debug.Log("Start Attack!");

            // 돌진 목표 위치 설정
            dashTarget = playerTransform.position;
            dashStartTime = Time.time;

            Vector2 direction = new Vector2(dashTarget.x - transform.position.x, 0).normalized;
            rb.velocity = direction * chaseSpeed;
            nextAttackTime = Time.time + 1f / attackRate; // 공격 후 쿨타임 적용
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
        rb.velocity = Vector2.zero; // 멈춤
        Debug.Log("Stop Attack!");
    }

    void SpecialAttack()
    {
        // 특수 공격 로직 구현
        Debug.Log("Boss Special Attack!");

        isSpecialAttacking = true; // 특수 공격 중 상태로 변경

        // 1. 플레이어 방향으로 짧게 돌진
        StartCoroutine(SpecialAttackDash());
    }

    System.Collections.IEnumerator SpecialAttackDash()
    {
        // 돌진
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

            // 투사체 여러 발 발사
            StartCoroutine(SpecialAttackShoot());
        }
        isSpecialAttacking = false;
    }

    System.Collections.IEnumerator SpecialAttackShoot()
    {
        // 투사체 여러 발 발사

        // 플레이어와 보스 사이의 각도 계산
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
                // 투사체 생성 위치 조정
                Vector3 spawnPosition = firePoint.position + (Vector3)direction * 1f; // firePoint에서 조금 더 바깥쪽으로 생성
                projectile.transform.position = spawnPosition;
                projectile.transform.rotation = Quaternion.identity;

                Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();

                if (projectileRb != null)
                {
                    projectileRb.velocity = direction * 5f; // 투사체 속도 설정
                    projectile.SetActive(true);
                }
                else
                    Debug.LogError("투사체에 rigidbody가 없습니다");
            }
            else
                Debug.LogError("ObjectPool에 투사체가 없습니다");

            Debug.Log("Projectile 발사!");
            yield return null; // 투사체 발사 간격
        }

        isSpecialAttacking = false; // 특수 공격 종료
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Boss 데미지 받음! 남은 체력: " + currentHealth);
    }

    void Die()
    {
        Debug.Log("Boss Die!");
        Destroy(gameObject); // 또는 오브젝트 풀에 반환
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
