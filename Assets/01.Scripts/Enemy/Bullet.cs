using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public int damageAmount = 10;

    private Vector2 direction = Vector2.zero; // 이동 방향 (기본값: (0,0))

    private Rigidbody2D rb; // Rigidbody2D 컴포넌트

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found on Bullet!");
        }
    }

    // OnEnable 삭제!

    void FixedUpdate()
    {
        rb.velocity = direction * speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("충돌 감지: " + collision.gameObject.name);
        LayerMask targetLayers = LayerMask.GetMask("Bullet", "Enemy", "Camera");
        if ((targetLayers.value & (1 << collision.gameObject.layer)) != 0)
        {
            // 다른 투사체와 충돌했으므로 무시

            return;
        }
        // 충돌 후 오브젝트 풀로 반환
        ReturnToPool();
    }

    void ReturnToPool()
    {
        // Debug.Log("ReturnToPool 호출");
        gameObject.SetActive(false);
        rb.velocity = Vector2.zero;
        ObjectPool.Instance.ReturnObjectToPool(gameObject);
    }

    public void SetDirection(Vector2 direction)
    {
        this.direction = direction.normalized; // 방향 정규화
        // Debug.Log("Set Direction: " + direction);
    }


}

