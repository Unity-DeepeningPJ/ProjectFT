using UnityEngine;

public class Boss : MeleeEnemy, IDamageable
{
    [Header("Special Attack")]
    public float specialAttackInterval = 10f; // 특수 공격 간격

    [Header("Dash Attack")]
    public float dashAttackDashSpeed = 7f; // 돌진 공격 속도
    public float dashAttackDashDistance = 3f; // 돌진 공격 거리

    [Header("Spread Shot")]
    public int spreadShotProjectileCount = 5; // 확산 투사체 발사 개수


    [Header("Offset Round Shot")]
    public int roundShotProjectileCount = 12; // 전방위 투사체 발사 개수
    public float roundShotDelay = 0.2f; // 투사체 발사 간격
    public int offsetRoundShotProjectileCount = 6; // 한쪽 면에서 발사할 투사체 개수
    public float offsetRoundShotSideOffset = 2f; // 보스 중심으로부터의 거리
    public float yOffset = 0.5f;

    [Header("Fan Shot")]
    public int fanShotProjectileCount = 6;   // 부채꼴 발사 투사체 개수
    public float fanShotSpreadAngle = 60f;   // 부채꼴 총 확산 각도
    public float fanShotJumpPower = 5f;      // 점프 힘

    [Header("Projectile")]
    public GameObject projectilePrefab; // 투사체 프리팹
    public float projectileSpeed = 5f; // 투사체 속도
    public float maxDetectionDistance = 10f; // 최대 감지 거리

    [Header("References")]
    public Transform firePoint; // 투사체 발사 위치

    private float nextSpecialAttackTime;
    private bool isSpecialAttacking = false; // 특수 공격 중인지 여부

    public enum SpecialAttackType
    {
        DashAttack,
        RoundShot,
        FanShot
    }




    public Boss(int Power, int Defense, int health, float speed, float jumpPower) : base(Power, Defense, health, speed, jumpPower)
    {
    }



    void Start()
    {


        currentHealth = maxHealth;
        nextSpecialAttackTime = Time.time + specialAttackInterval;
        startPosition = transform.position;
        nextAttackTime = Time.time;

        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found!");
        }
        transform.localScale = new Vector3(1, 1, 1); // 보스 크기 키우기
        moveSpeed = 0f; // 보스 이동 속도 0으로 설정
        chaseSpeed = 0f; // 보스 추격 속도 0으로 설정

        // 플레이어 Transform 찾기
        GameObject player = FindClosestObjectWithLayer(transform.position, playerLayer, maxDetectionDistance);
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            //Debug.LogError("Player not found in layer!");
        }
    }

    void Update()
    {
        GameObject player = FindClosestObjectWithLayer(transform.position, playerLayer, maxDetectionDistance);
        if (player != null)
        {
            playerTransform = player.transform;            
        }
        else
        {
            playerTransform = null;            
        }
        if (currentHealth <= 0)
        {
            Die();
        }

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
            ChooseSpecialAttack();
            nextSpecialAttackTime = Time.time + specialAttackInterval;
        }
    }

    void HandleIdleState()
    {
        if (Time.time >= stopTime)
        {
            isIdle = false;
            StopAttack();
        }
    }
    void HandleMovementState()
    {
        if (playerTransform != null)
        {
            StartAttack();
        }
    }
    void StartAttack()
    {
        if (Time.time >= nextAttackTime && !isSpecialAttacking && playerTransform != null)
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
    }

    void ChooseSpecialAttack()
    {
        SpecialAttackType attackType = (SpecialAttackType)Random.Range(0, System.Enum.GetValues(typeof(SpecialAttackType)).Length);

        switch (attackType)
        {
            case SpecialAttackType.DashAttack:
                StartCoroutine(DashAttack());
                break;
            case SpecialAttackType.RoundShot:
                StartCoroutine(RoundShot());
                break;
            case SpecialAttackType.FanShot:
                StartCoroutine(FanShot());
                break;
        }
    }

    System.Collections.IEnumerator DashAttack()
    {
        isSpecialAttacking = true; // 특수 공격 중 상태로 변경
        animi.OnAttack(false);

        // 돌진
        if (playerTransform != null)
        {
            Vector2 direction = new Vector2(playerTransform.position.x - transform.position.x, 0).normalized;
            rb.velocity = direction * dashAttackDashSpeed;

            float timer = 0;
            while (timer < dashDuration)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            rb.velocity = Vector2.zero;

            // 투사체 여러 발 발사
            StartCoroutine(SpreadShot());
        }
        isSpecialAttacking = false;
    }

    System.Collections.IEnumerator SpreadShot()
    {        
        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;        
        animi.OnAttack(true);

        for (int i = 0; i < spreadShotProjectileCount; i++)
        {
            GameObject projectile = ObjectPool.Instance.GetPooledObject();

            if (projectile != null)
            {
                // 투사체 생성 위치 조정
                Vector3 spawnPosition = firePoint.position;
                projectile.transform.position = spawnPosition;
                projectile.transform.rotation = Quaternion.identity;

                // 투사체 초기화 (OnEnable()에서 처리하는 것이 더 좋음)
                Bullet bulletScript = projectile.GetComponent<Bullet>();
                if (bulletScript != null)
                {
                    bulletScript.SetDirection(directionToPlayer); // 플레이어 방향으로 설정
                }
                else
                {
                    Debug.LogError("Bullet script not found on projectile!");
                }

                Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();

                if (projectileRb != null)
                {
                    projectileRb.velocity = directionToPlayer * projectileSpeed; // 투사체 속도 설정                    
                    projectile.SetActive(true);                    
                }
                else
                    Debug.LogError("투사체에 rigidbody가 없습니다");
            }
            else
                Debug.LogError("ObjectPool에 투사체가 없습니다");

            yield return new WaitForSeconds(0.05f);
        }

        isSpecialAttacking = false;
    }

    System.Collections.IEnumerator RoundShot()
    {        
        isSpecialAttacking = true; // 특수 공격 중 상태로 변경

        // 플레이어의 위치를 기준으로 보스의 왼쪽 또는 오른쪽 선택        
        float side = playerTransform.position.x > transform.position.x ? 1f : -1f; // 플레이어가 오른쪽에 있으면 1, 왼쪽에 있으면 -1

        // 플레이어 방향과 반대 방향으로 offset 설정
        Vector3 offset = new Vector3(side * offsetRoundShotSideOffset, 0, 0); // 좌우 offset

        float angleStep = 180f / (offsetRoundShotProjectileCount - 1); // 180도 반원

        for (int i = 0; i < offsetRoundShotProjectileCount; i++)
        {
            float angle = i * angleStep; // 0도에서 180도까지

            // 플레이어 반대방향으로 회전
            Vector2 direction = Quaternion.AngleAxis(angle - 90, Vector3.forward) * (side * Vector2.right); // right = (1,0)

            GameObject projectile = ObjectPool.Instance.GetPooledObject();

            if (projectile != null)
            {
                // 투사체 생성 위치 조정
                Vector3 spawnPosition = firePoint.position + offset + Vector3.up * yOffset;                
                projectile.transform.position = spawnPosition;
                projectile.transform.rotation = Quaternion.identity;

                Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();

                if (projectileRb != null)
                {
                    projectileRb.velocity = direction * projectileSpeed; // 투사체 속도 설정
                                                                         // Bullet 스크립트에 방향 전달
                    Bullet bulletScript = projectile.GetComponent<Bullet>();
                    if (bulletScript != null)
                    {
                        bulletScript.SetDirection(direction);
                    }

                    projectile.SetActive(true);
                }
                else
                    Debug.LogError("투사체에 rigidbody가 없습니다");
            }
            else
                Debug.LogError("ObjectPool에 투사체가 없습니다");

            yield return new WaitForSeconds(roundShotDelay); // 투사체 발사 간격
        }
        isSpecialAttacking = false;
    }
    System.Collections.IEnumerator FanShot()
    {        
        isSpecialAttacking = true;

        if (playerTransform == null)
        {            
            isSpecialAttacking = false;
            yield break; // 플레이어가 없으면 코루틴 종료
        }

        rb.AddForce(Vector2.up * fanShotJumpPower, ForceMode2D.Impulse); // 위쪽으로 힘을 가해서 점프
        yield return new WaitForSeconds(0.3f); // 점프 후 0.3초 정도 대기         


        Vector2 centerDirection = (playerTransform.position - firePoint.position).normalized;
        float startAngle = -fanShotSpreadAngle / 2f;
        float angleStep = (fanShotProjectileCount > 1) ? fanShotSpreadAngle / (fanShotProjectileCount - 1) : 0f;

        // 4. 투사체 발사
        for (int i = 0; i < fanShotProjectileCount; i++)
        {
            // 현재 투사체의 발사 각도 계산
            float currentAngle = startAngle + (i * angleStep);

            // 중심 방향 벡터를 현재 각도만큼 회전시켜 최종 발사 방향 계산
            Vector2 fireDirection = Quaternion.AngleAxis(currentAngle, Vector3.forward) * centerDirection;

            // 오브젝트 풀에서 투사체 가져오기
            GameObject projectile = ObjectPool.Instance.GetPooledObject();

            if (projectile != null)
            {
                projectile.transform.position = firePoint.position;
                // 선택 사항: 투사체가 발사 방향을 바라보도록 회전
                projectile.transform.rotation = Quaternion.LookRotation(Vector3.forward, fireDirection);

                Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
                Bullet bulletScript = projectile.GetComponent<Bullet>(); // Bullet 스크립트 가져오기

                if (projectileRb != null && bulletScript != null)
                {
                    bulletScript.SetDirection(fireDirection);
                    projectileRb.velocity = fireDirection * projectileSpeed;// Rigidbody 속도 설정
                    projectile.SetActive(true);

                }
                else
                {
                    projectile.SetActive(false);
                }
            }

        }

        yield return new WaitForSeconds(0.1f); // 아주 짧은 후딜레이        
        isSpecialAttacking = false; // 특수 공격 종료

    }
    public void TakePhysicalDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
            AudioManager.Instance.PlayBGM("Test");
        }
    }

    void Die()
    {
        for (int i = 0; i < 5; i++)
        {
            Vector3 spawnPos = new Vector3(
                Random.Range(transform.position.x - 4f, transform.position.x + 4f),
                transform.position.y + 3,
                transform.position.z
            );
            Instantiate(coinPrefab, spawnPos, Quaternion.identity);
        }

        Destroy(gameObject); // 또는 오브젝트 풀에 반환

        GameObject npcObj = GameObject.FindGameObjectWithTag("NPC");
        ShopkeeperNPC npc = npcObj.GetComponent<ShopkeeperNPC>();
        npc.MoveNPC();


        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // PlayerState 컴포넌트 가져오기
            PlayerState playerState = player.GetComponent<PlayerState>();

            if (playerState != null)
            {
                // 레벨업 로직 실행
                playerState.LevelUp();
            }
        }
    }

    GameObject FindClosestObjectWithLayer(Vector3 position, LayerMask playerLayer, float maxDistance)
    {
        GameObject closest = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 pos = position;

        // 지정된 레이어 마스크에 해당하는 모든 GameObject 찾기
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, maxDistance, playerLayer);

        foreach (Collider2D collider in colliders)
        {
            GameObject go = collider.gameObject;
            Vector3 diff = go.transform.position - pos;
            float distanceSqr = diff.sqrMagnitude;

            if (distanceSqr < closestDistanceSqr)
            {
                closest = go;
                closestDistanceSqr = distanceSqr;
            }
        }

        // 만약 플레이어가 maxDistance 내에 없다면 closest는 null을 반환
        if (closestDistanceSqr == Mathf.Infinity) // 거리가 초기값과 같다면, 플레이어를 찾지 못했다는 의미
        {
            return null; // 플레이어를 찾지 못했으면 null 반환
        }


        return closest;
    }


}
