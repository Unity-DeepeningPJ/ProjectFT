using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BaseState, IDamageable
{
    public float moveDistance = 2f; // 이동 거리
    public float moveSpeed = 2f; // 이동 속도
    public float fireRate = 2f; // 발사 간격
    public Transform firePoint; // 투사체 발사 위치
    public LayerMask playerLayer; // 플레이어 레이어
    public float detectionRange = 5f; // 플레이어 인식 범위
    public int maxHealth = 200;  
    private int currentHealth;

    //private int 
    private Vector2 startPosition;
    private bool movingRight = true;
    private float nextFireTime;

    private Transform playerTransform; // 플레이어의 Transform
    private bool playerInRange = false; // 플레이어가 범위 내에 있는지 여부

    public Enemy(int Power, int Defense, int health, float speed, float jumpPower) : base(Power, Defense, health, speed, jumpPower)
    {
        
    }

    void Start()
    {
        startPosition = transform.position;
        nextFireTime = Time.time;
        currentHealth = maxHealth;

    }

    void Update()
    {
        Move();
        FindPlayer(); // 매 프레임마다 플레이어 감지
               
        if (playerInRange && Time.time >= nextFireTime) // 플레이어가 범위 내에 있고 발사 시간이 되었을 때만 발사
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;           
        }
        if (currentHealth <= 0) // 현재 체력이 0 이하가 되면
        {
            Die(); // Die() 함수 호출
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
    }

    void Shoot()
    {
        GameObject projectile = ObjectPool.Instance.GetPooledObject();
        if (projectile != null)
        {
            projectile.transform.position = firePoint.position;
            // 투사체 발사 방향 설정 (플레이어 위치로)
            Vector2 direction = (playerTransform.position - firePoint.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // 투사체 발사 (플레이어 방향으로)
            Bullet projectileScript = projectile.GetComponent<Bullet>();
            if (projectileScript != null)
            {
                projectileScript.SetDirection(direction);                
            }

            projectile.SetActive(true);
            
        }
        else
        {
            Debug.LogWarning("Projectile is null from Object Pool!"); // 오브젝트 풀에서 투사체를 가져오지 못함
        }
    }

    void FindPlayer()
    {    
                
        GameObject player = FindClosestObjectWithLayerInRange(transform.position, playerLayer, detectionRange);

        if (player != null)
        {
            playerTransform = player.transform;
            playerInRange = true;             
        }
        else
        {
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
            // 3. 오브젝트 레이어와 플레이어 레이어 마스크 비교
            if ((layer.value & (1 << go.layer)) != 0)
            {
                Vector3 diff = go.transform.position - pos;
                float curDistance = diff.sqrMagnitude;

                // 4. 거리와 범위 확인
                if (curDistance < distance && curDistance <= range * range) // 범위 내에 있는지 확인
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
        Debug.Log("Enemy Die!");
        Destroy(gameObject); 
    }
}
