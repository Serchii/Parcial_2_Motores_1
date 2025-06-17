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
            Invoke("DisableInvincible", 1.5f);
        }

        OnHealthChanged?.Invoke(health, maxHealth);
        Debug.Log($"{gameObject.name}: Recibí daño.");
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
            Invoke("Respawn", 3f);
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
}
