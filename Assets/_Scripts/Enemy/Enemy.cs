using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BaseState
{
    public float moveDistance = 2f; // �̵� �Ÿ�
    public float moveSpeed = 2f; // �̵� �ӵ�
    public float fireRate = 2f; // �߻� ����
    public Transform firePoint; // ����ü �߻� ��ġ
    public LayerMask playerLayer; // �÷��̾� ���̾�
    public float detectionRange = 5f; // �÷��̾� �ν� ����

    private Vector2 startPosition;
    private bool movingRight = true;
    private float nextFireTime;

    private Transform playerTransform; // �÷��̾��� Transform
    private bool playerInRange = false; // �÷��̾ ���� ���� �ִ��� ����

    public Enemy(int Power, int Defense, int health, float speed, float jumpPower) : base(Power, Defense, health, speed, jumpPower)
    {
    }

    void Start()
    {
        startPosition = transform.position;
        nextFireTime = Time.time;
        
    }

    void Update()
    {
        Move();
        FindPlayer(); // �� �����Ӹ��� �÷��̾� ����
               
        if (playerInRange && Time.time >= nextFireTime) // �÷��̾ ���� ���� �ְ� �߻� �ð��� �Ǿ��� ���� �߻�
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;           
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
            // ����ü �߻� ���� ���� (�÷��̾� ��ġ��)
            Vector2 direction = (playerTransform.position - firePoint.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // ����ü �߻� (�÷��̾� ��������)
            Bullet projectileScript = projectile.GetComponent<Bullet>();
            if (projectileScript != null)
            {
                projectileScript.SetDirection(direction);                
            }

            projectile.SetActive(true);
            
        }
        else
        {
            Debug.LogWarning("Projectile is null from Object Pool!"); // ������Ʈ Ǯ���� ����ü�� �������� ����
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
            // 3. ������Ʈ ���̾�� �÷��̾� ���̾� ����ũ ��
            if ((layer.value & (1 << go.layer)) != 0)
            {
                Vector3 diff = go.transform.position - pos;
                float curDistance = diff.sqrMagnitude;

                // 4. �Ÿ��� ���� Ȯ��
                if (curDistance < distance && curDistance <= range * range) // ���� ���� �ִ��� Ȯ��
                {
                    closest = go;
                    distance = curDistance;
                    
                }
            }
        }
        return closest;
    }
}
