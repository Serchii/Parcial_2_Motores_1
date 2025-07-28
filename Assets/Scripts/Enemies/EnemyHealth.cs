using UnityEngine;

public class EnemyHealth : BaseHealth
{
    
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float stunDuration = 0.2f;
    [SerializeField] int maxMoney = 15;

    [Header("Health Bar")]
    [SerializeField] EnemyHealthBarUI healthBarPrefab;
    [SerializeField] private EnemyHealthBarUI healthBarInstance;
    [SerializeField] EnemyBehaviour enemyBehaviour;
    public bool IsDead => isDead;

    protected override void Start()
    {
        base.Start();

        if (_animator == null)
            _animator = GetComponent<Animator>();

        if (_rb == null)
            _rb = GetComponent<Rigidbody2D>();

        if (enemyBehaviour == null)
            enemyBehaviour = GetComponent<EnemyBehaviour>();
            
        Canvas canvas = FindObjectOfType<Canvas>();
        healthBarInstance = Instantiate(healthBarPrefab, canvas.transform);
        healthBarInstance.SetTarget(transform);
    }

    public void TakeDamage(float amount, Vector2 knockbackDirection, float knockbackForce, bool getStunned)
    {
        if (isDead) return;

        health -= amount;

        healthBarInstance.SetHealth(health, maxHealth);

        if (getStunned)
        {
            enemyBehaviour?.Stun(stunDuration);
        }

        ApplyKnockback(knockbackDirection, knockbackForce);

        if (_animator != null && (!enemyBehaviour.IsAttacking || health <= 0))
        {
            _animator.SetTrigger("Hurt");
            enemyBehaviour.EndAttack();
        }

        if (health <= 0f)
        {
            health = 0f;
            Die();
        }
    }

    public override void Die()
    {
        if (isDead) return;

        isDead = true;

        _rb.bodyType = RigidbodyType2D.Static;
        GetComponent<BoxCollider2D>().enabled = false;
        if (_animator != null)
        {
            _animator.SetBool("IsDead", isDead);
        }

        GameManager.Instance.AddMoney(Random.Range(0, maxMoney + 1));
        Destroy(gameObject, 1.5f);
    }

    private void ApplyKnockback(Vector2 direction, float force)
    {
        if (isDead || _rb == null) return;

        _rb.velocity = Vector2.zero;
        Debug.Log($"Direccion: {direction}. Fuerza: {force}");
        _rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);
    }
}