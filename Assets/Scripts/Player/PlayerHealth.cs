using System;
using UnityEngine;

public class PlayerHealth : BaseHealth
{
    [SerializeField] bool isAlive = true;
    [SerializeField] int lives = 1;
    [SerializeField] Vector3 spawnPoint;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] bool invincible = false;

    public float Health => health;
    public float MaxHealth => maxHealth;
    public bool IsAlive => isAlive;
    public int Lives => lives;
    public bool Invincible => invincible;

    public event Action<float, float> OnHealthChanged;
    public event Action<int> OnLivesChanged;

    protected override void Start()
    {
        base.Start();
        spawnPoint = transform.position;

        if (animator == null)
            animator = GetComponent<Animator>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (GameManager.Instance != null)
        {
            health = GameManager.Instance.SavedHealth;
            maxHealth = GameManager.Instance.SavedMaxHealth;
        }
    }

    public override void TakeDamage(float amount)
    {
        if (!isAlive || invincible) return;

        health -= amount;

        if (animator != null)
            animator.SetTrigger("Hurt");

        if (health <= 0f)
        {
            health = 0f;
            if (isAlive) Die();
        }
        else
        {
            EnableInvincible();
            Invoke(nameof(DisableInvincible), 1.5f);
        }

        OnHealthChanged?.Invoke(health, maxHealth);
        Debug.Log($"{gameObject.name}: Recibió daño.");
    }

    public override void Die()
    {
        if (!isAlive) return;

        lives--;
        OnLivesChanged?.Invoke(lives);
        isAlive = false;

        Debug.Log($"{gameObject.name}: Me morí.");

        if (lives > 0)
        {
            Invoke(nameof(Respawn), 3f);
        }
        else
        {
            GameManager.Instance.PlayerDied();
        }
    }

    private void Respawn()
    {
        SetMaxHealth();
        transform.position = spawnPoint;
        isAlive = true;
    }

    protected override void SetMaxHealth()
    {
        base.SetMaxHealth();
        OnHealthChanged?.Invoke(health, maxHealth);
    }

    public void SetMaxHealthValue(float value)
    {
        maxHealth = value;
        SetMaxHealth();
    }

    private void DisableInvincible()
    {
        invincible = false;
        Color color = spriteRenderer.color;
        color.a = 1f;
        spriteRenderer.color = color;
    }

    private void EnableInvincible()
    {
        invincible = true;
        Color color = spriteRenderer.color;
        color.a = 0.5f;
        spriteRenderer.color = color;
    }

    public void ApplyKnockback(Vector2 direction, float force)
    {
        if (!isAlive || invincible) return;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction.normalized * force;

            PlayerMovement pm = GetComponent<PlayerMovement>();
            if (pm != null)
            {
                pm.TriggerKnockback(pm.knockbackDuration);
            }
        }
    }

}
