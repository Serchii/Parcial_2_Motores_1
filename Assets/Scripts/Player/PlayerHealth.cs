using System;
using UnityEngine;

public class PlayerHealth : BaseHealth
{
    [SerializeField] bool isAlive = true;
    [SerializeField] int lives = 1;
    [SerializeField] Vector3 spawnPoint;
    [SerializeField] Animator animator;
    [SerializeField] PlayerMovement pm;
    [SerializeField] private DamageFlash damageFlash;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] bool invincible = false;

    public float Health => health;
    public float MaxHealth => maxHealth;
    public bool IsAlive => isAlive;
    public int Lives => lives;
    public bool Invincible => invincible;

    public event Action<float, float> OnHealthChanged;
    public event Action<int> OnLivesChanged;
    public event Action OnFlash;
    public event Action OnShake;

    protected override void Start()
    {
        spawnPoint = transform.position;

        if (pm == null)
            pm = GetComponent<PlayerMovement>();

        if (animator == null)
            animator = GetComponent<Animator>();

        if (damageFlash == null)
            damageFlash = GetComponentInChildren<DamageFlash>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (GameManager.Instance != null)
        {
            health = GameManager.Instance.SavedHealth;
            maxHealth = GameManager.Instance.SavedMaxHealth;
        }

        OnHealthChanged?.Invoke(health, maxHealth);

        //ApplyUpgrades();
    }

    public override void TakeDamage(float amount)
    {
        if (!isAlive || invincible) return;

        health -= amount;

        if (damageFlash != null)
        {
            damageFlash.Flash();
            OnShake?.Invoke();
        }

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
    }

    public override void Die()
    {
        if (!isAlive) return;

        lives--;
        OnLivesChanged?.Invoke(lives);
        isAlive = false;

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
        Debug.Log("SetMaxHealthPlayer");
        OnHealthChanged?.Invoke(health, maxHealth);
    }

    public void SetMaxHealthValue(float value)
    {
        Debug.Log("SetMaxHealthValue");
        maxHealth = value;
        SetMaxHealth();
    }

    public void DisableInvincible()
    {
        invincible = false;
        Color color = spriteRenderer.color;
        color.a = 1f;
        spriteRenderer.color = color;
        Debug.Log("DisableInvincible");
    }

    public void EnableInvincible()
    {
        invincible = true;
        Color color = spriteRenderer.color;
        color.a = 0.5f;
        spriteRenderer.color = color;
        Debug.Log("EnableInvincible");
    }

    public void ApplyKnockback(Vector2 direction, float force)
    {
        if (!isAlive) return;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(new Vector2(direction.x * force, 0f), ForceMode2D.Impulse);

            
            if (pm != null)
            {
                pm.TriggerKnockback(pm.knockbackDuration);
            }
        }
    }

    void ApplyUpgrades()
    {
        if (PlayerInventory.Instance != null)
        {
            if (PlayerInventory.Instance.HasItem(ItemID.HelmetUltimate))
                SetMaxHealthValue(200f);
            else if (PlayerInventory.Instance.HasItem(ItemID.HelmetImproved))
                SetMaxHealthValue(150f);
            else
                SetMaxHealthValue(100f);
        }
    }
}
