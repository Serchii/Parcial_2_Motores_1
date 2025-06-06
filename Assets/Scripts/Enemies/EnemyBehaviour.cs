using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _chaseRange = 5f;
    [SerializeField] private float _attackRange = 1.2f;

    [Header("Ataque")]
    [SerializeField] private float _attackCooldown = 1f;
    [SerializeField] private float _damage = 10f;

    [Header("Detección")]
    [SerializeField] private Transform _player;

    private Rigidbody2D _rb;
    private Animator _animator;
    private EnemyHealth _health; // <-- nueva referencia
    private float _nextAttackTime = 0f;
    private bool _isFacingRight = true;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _health = GetComponent<EnemyHealth>(); // <-- inicializar

        if (_player == null && GameObject.FindGameObjectWithTag("Player") != null)
        {
            _player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        if (_player == null || (_health != null && _health.IsDead)) return; // <-- evitar actuar si está muerto

        float distanceToPlayer = Vector2.Distance(transform.position, _player.position);

        if (distanceToPlayer <= _attackRange)
        {
            _rb.velocity = Vector2.zero;
            _animator.SetBool("IsRunning", false);
            TryAttack();
        }
        else if (distanceToPlayer <= _chaseRange)
        {
            MoveTowardsPlayer();
        }
        else
        {
            _rb.velocity = Vector2.zero;
            _animator.SetBool("IsRunning", false);
        }

        FlipSprite();
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = (_player.position - transform.position).normalized;
        _rb.velocity = new Vector2(direction.x * _moveSpeed, _rb.velocity.y);
        _animator.SetBool("IsRunning", true);
    }

    private void TryAttack()
    {
        if (Time.time >= _nextAttackTime)
        {
            _nextAttackTime = Time.time + _attackCooldown;
            _animator.SetTrigger("IsAttack");

            IDamageable damageable = _player.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(_damage);
            }
        }
    }

    private void FlipSprite()
    {
        if (_player == null) return;

        if ((_player.position.x < transform.position.x && _isFacingRight) ||
            (_player.position.x > transform.position.x && !_isFacingRight))
        {
            _isFacingRight = !_isFacingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}