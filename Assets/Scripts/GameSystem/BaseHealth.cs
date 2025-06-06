using UnityEngine;

public class BaseHealth : MonoBehaviour, IDamageable
{
    [SerializeField] protected float health = 100f;
    [SerializeField] protected float maxHealth = 100f;

    protected bool isDead = false;

    // Método Start virtual para que PlayerHealth pueda sobreescribirlo
    protected virtual void Start()
    {
        SetMaxHealth();
    }

    public virtual void TakeDamage(float amount)
    {
        if (isDead) return;

        health -= amount;

        if (health <= 0f)
        {
            health = 0f;
            Die();
        }
    }

    public virtual void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log($"{gameObject.name} murió.");
        Destroy(gameObject, 1.5f);
    }

    protected virtual void SetMaxHealth()
    {
        health = maxHealth;
    }
}
