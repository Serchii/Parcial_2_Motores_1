using UnityEngine;

public abstract class BaseHealth : MonoBehaviour, IDamageable
{
    [SerializeField] protected float maxHealth = 100f;
    [SerializeField] protected float health;

    public virtual float Health => health;
    public virtual float MaxHealth => maxHealth;
    
    protected virtual void Start()
    {
        SetMaxHealth();
    }

    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log($"{gameObject.name}: Recibi {amount} de daño");

        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name}: Me mori.");
        Destroy(gameObject);
    }

    protected virtual void SetMaxHealth()
    {
        health = maxHealth;
    }
}
