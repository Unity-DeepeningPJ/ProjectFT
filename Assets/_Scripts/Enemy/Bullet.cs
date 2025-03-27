using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public float lifeTime = 2f;
    public LayerMask playerLayer; // �÷��̾� ���̾�

    private Vector2 direction = Vector2.right; // �̵� ���� (�⺻��: ������)
    private Transform playerTransform; // �÷��̾� Transform
    private float currentLifeTime;

    private Rigidbody2D rb; // Rigidbody2D ������Ʈ

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
        currentLifeTime = 0f;
        rb.velocity = Vector2.zero;

        // �÷��̾� Transform ã��
        GameObject player = FindClosestObjectWithLayer(transform.position, playerLayer); // LayerMask�� ����Ͽ� �÷��̾� ã��
        if (player != null)
        {
            playerTransform = player.transform;
            SetDirection((playerTransform.position - transform.position).normalized);
        }
        else
        {
            Debug.LogWarning("Player not found in layer. Projectile will move to the right.");
            direction = Vector2.right; // �÷��̾ ã�� ���ϸ� ���������� �̵�
        }
    }

    void Update()
    {
        currentLifeTime += Time.deltaTime;

        if (currentLifeTime >= lifeTime)
        {
            ReturnToPool();
        }
    }

    void FixedUpdate()
    {
        rb.velocity = direction * speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // �浹 ó�� ���� (��: �÷��̾�� ������)
        // ������ ���� ���� �ʿ�

        // �浹 �� ������Ʈ Ǯ�� ��ȯ
        ReturnToPool();
    }

    void ReturnToPool()
    {
        Debug.Log("ReturnToPool ȣ��");
        gameObject.SetActive(false);
        rb.velocity = Vector2.zero;
        ObjectPool.Instance.ReturnObjectToPool(gameObject);
    }

    public void SetDirection(Vector2 direction)
    {
        this.direction = direction.normalized; // ���� ����ȭ
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

