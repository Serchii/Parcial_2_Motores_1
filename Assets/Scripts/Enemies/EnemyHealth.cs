using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float _health = 50f;
    [SerializeField] private Animator _animator;

    private bool _isDead = false;
    private Rigidbody2D _rb;
    private Collider2D _collider;

    public bool IsDead => _isDead;

    private void Start()
    {
        if (_animator == null)
            _animator = GetComponent<Animator>();

        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    public void TakeDamage(float amount)
    {
        if (_isDead) return;

        _health -= amount;
        Debug.Log($"{gameObject.name} recibió {amount} de daño. Vida restante: {_health}");

        if (_health <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        if (_isDead) return;

        _isDead = true;

        if (_animator != null)
            _animator.SetTrigger("IsDead");

        if (_rb != null)
        {
            _rb.velocity = Vector2.zero;
            _rb.bodyType = RigidbodyType2D.Static;
        }

        if (_collider != null)
            _collider.enabled = false;

        Destroy(gameObject, 1.5f);
    }
}