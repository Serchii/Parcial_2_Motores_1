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
    private float _nextAttackTime = 0f;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        if (_player == null)
            _player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (_player == null) return;

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
                Debug.Log($"{gameObject.name}: ataque a {_player.name}");
                damageable.TakeDamage(_damage);
            }
        }
    }

    private void FlipSprite()
    {
        if (_player == null) return;

        Vector3 scale = transform.localScale;

        if (_player.position.x < transform.position.x)
        {
            // Jugador a la izquierda, sprite mirando a la izquierda ? escala positiva
            if (scale.x < 0)
                scale.x *= -1;
        }
        else
        {
            // Jugador a la derecha, sprite mirando a la derecha ? escala negativa
            if (scale.x > 0)
                scale.x *= -1;
        }

        transform.localScale = scale;
    }
}