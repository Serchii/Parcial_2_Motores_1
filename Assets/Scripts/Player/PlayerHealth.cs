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
        
        if(health > 0 )
        {
            health -= amount;
        }

        if (health <= 0)
        {
            health = 0;
            if(isAlive) Die();
        }

        OnHealthChanged?.Invoke(health, maxHealth);
        Debug.Log($"{gameObject.name}: Recibi daño.");
    }

    protected override void Die()
    {
        lives--;
        OnLivesChanged?.Invoke(lives);

        isAlive = false;
        //animator.SetBool("IsDead",!isAlive);
        Debug.Log($"{gameObject.name}: Me mori.");

        if (lives > 0)
        {
            // Reaparece después de 3 segundos
            Invoke("Respawn", 3f);
        }
        else
        {
            GameManager.Instance.PlayerDied();
            Debug.Log("Morimos");
        }
    }

    private void Respawn()
    {

        SetMaxHealth();

        if (spawnPoint != null)
        {
            transform.position = spawnPoint;
        }

        isAlive = true;
        //animator.SetBool("IsDead",!isAlive);
    }

    protected override void SetMaxHealth()
    {
        base.SetMaxHealth();
        OnHealthChanged?.Invoke(health, maxHealth);
    }
}