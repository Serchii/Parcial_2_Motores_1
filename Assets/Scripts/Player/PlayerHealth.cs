using System;
using UnityEngine;

public class PlayerHealth : BaseHealth
{
    [SerializeField] bool isAlive = true;
    [SerializeField] int lives = 3;
    [SerializeField] Vector3 spawnPoint;
    [SerializeField] Animator animator;

    public bool IsAlive => isAlive;
    public int Lives => lives;

    public event Action<float, float> OnHealthChanged;
    public event Action<int> OnLivesChanged;

    protected override void Start()
    {
        base.Start();
        spawnPoint = transform.position;
    }

    public override void TakeDamage(float amount)
    {
        if (!isAlive) return;

        health -= amount;

        if (health <= 0f)
        {
            health = 0f;
            if (isAlive) Die();
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
}