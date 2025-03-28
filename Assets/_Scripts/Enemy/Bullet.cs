using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;    
    public LayerMask targetLayer; // 플레이어 레이어
    public int damageAmount = 10;

    private Vector2 direction = Vector2.right; // 이동 방향 (기본값: 오른쪽)
    private Transform playerTransform; // 플레이어 Transform
    

    private Rigidbody2D rb; // Rigidbody2D 컴포넌트

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found on Bullet!");
        }
    }

    void OnEnable()
    {
        
        rb.velocity = Vector2.zero;

        // 플레이어 Transform 찾기
        GameObject player = FindClosestObjectWithLayer(transform.position, targetLayer); // LayerMask를 사용하여 플레이어 찾기
        if (player != null)
        {
            playerTransform = player.transform;
            SetDirection((playerTransform.position - transform.position).normalized);
        }
        else
        {
            Debug.LogWarning("Player not found in layer. Projectile will move to the right.");
            direction = Vector2.right; // 플레이어를 찾지 못하면 오른쪽으로 이동
        }
    }

    

    void FixedUpdate()
    {
        rb.velocity = direction * speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (targetLayer == (targetLayer | (1 << collision.gameObject.layer)))
        {
            ReturnToPool();
        }
            // 충돌 후 오브젝트 풀로 반환
            ReturnToPool();
    }

    void ReturnToPool()
    {
        Debug.Log("ReturnToPool 호출");
        gameObject.SetActive(false);
        rb.velocity = Vector2.zero;
        ObjectPool.Instance.ReturnObjectToPool(gameObject);
    }

    public void SetDirection(Vector2 direction)
    {
        this.direction = direction.normalized; // 방향 정규화
        Debug.Log("Set Direction: " + direction);
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

