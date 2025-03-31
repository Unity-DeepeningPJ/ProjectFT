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

    //데미지 계산
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

    //데미지 받은 후 잠시 무적상태
    public IEnumerator InvincibilityFrames(float duration)
    {
        IsInvincible = true;
        IgnoreAllEnemyCollision(true);
        yield return new WaitForSeconds(duration);
        IsInvincible = false;
        IgnoreAllEnemyCollision(false);
    }

    public void ApplyKnockback(Vector2 direction, float force)
    {
        _player.StartCoroutine(ApplyKnockbackCoroutine(direction, force, 0.3f));
    }

    public IEnumerator ApplyKnockbackCoroutine(Vector2 direction, float force, float duration)
    {
        _player.Rigidbody.velocity = Vector2.zero;
        _player.Rigidbody.AddForce(direction * force, ForceMode2D.Impulse);

        _player.Controller.canMove = false;
        yield return new WaitForSeconds(duration);
        _player.Controller.canMove = true;
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
