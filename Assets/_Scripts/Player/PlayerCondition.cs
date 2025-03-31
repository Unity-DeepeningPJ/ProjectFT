using System.Collections;
using UnityEngine;

public class PlayerCondition : MonoBehaviour, IDamageable
{
    private Player _player;
    public float CurrentHealth { get; private set; }
    public bool IsInvincible { get; set; } = false;

    private Collider2D _playerCollider;

    public void Initizlize(Player player, float currentHealth)
    {
        _player = player;
        CurrentHealth = currentHealth;
        _playerCollider = GetComponent<Collider2D>();
    }

    public void TakePhysicalDamage(int damage)
    {
        if (IsInvincible) return;

        float finalDamage = Mathf.Max(damage - _player.PlayerState.TotalDefense, 1);
        CurrentHealth -= finalDamage;
        
        if(CurrentHealth <= 0)
        {
            Die();
        }
        else
        {
            _player.StartCoroutine(InvincibilityFrames(1f));
        }
        Debug.Log("현재 체력 : " + CurrentHealth);
    }

    public IEnumerator InvincibilityFrames(float duration)
    {
        Debug.Log("무적시작");
        IsInvincible = true;
        IgnoreAllEnemyCollision(true);
        yield return new WaitForSeconds(duration);
        Debug.Log("무적끝");
        IsInvincible = false;
        IgnoreAllEnemyCollision(false);
    }

    public void ApplyKnockback(Vector2 direction, float force)
    {
        _player.Rigidbody.AddForce(direction * force, ForceMode2D.Impulse);
    }

    private void IgnoreAllEnemyCollision(bool ignore)
    {
        Collider2D[] enemies = FindObjectsOfType<Collider2D>();

        foreach(Collider2D enemy in enemies)
        {
            if(enemy.gameObject.layer == LayerMask.NameToLayer("Enemy") || enemy.gameObject.layer == LayerMask.NameToLayer("Bullet"))
            {
                Physics2D.IgnoreCollision(enemy, _playerCollider, ignore);
            }
        }
    }

    private void Die()
    {
        Debug.Log("사망");
    }
}
