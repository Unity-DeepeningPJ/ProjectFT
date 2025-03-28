using System.Collections;
using UnityEngine;

public class PlayerCondition : MonoBehaviour, IDamageable
{
    private Player _player;
    public float CurrentHealth { get; private set; }
    public bool IsInvincible { get; set; } = false;

    private Collider2D _playerCollider;
    private Collider2D _enemyCollider;

    public PlayerCondition(Player player, float currentHealth)
    {
        _player = player;
        CurrentHealth = currentHealth;
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
            _player.StartCoroutine(InvincibilityFrames(_enemyCollider, _playerCollider, 1f));
        }
        Debug.Log("현재 체력 : " + CurrentHealth);
    }

    private IEnumerator InvincibilityFrames(Collider2D enemyCollider, Collider2D playerCollider, float duration)
    {
        IsInvincible = true;
        Physics2D.IgnoreCollision(enemyCollider, playerCollider, true);
        yield return new WaitForSeconds(duration);
        IsInvincible = false;
        Physics2D.IgnoreCollision(enemyCollider, playerCollider, false);
    }

    public void ApplyKnockback(Vector2 direction, float force)
    {
        _player.Rigidbody.AddForce(direction * force, ForceMode2D.Impulse);
    }

    public void SetCollider(Collider2D enemyCollider, Collider2D playerCollider)
    {
        _enemyCollider = enemyCollider;
        _playerCollider = playerCollider;
    }

    private void Die()
    {
        Debug.Log("사망");
    }
}
