using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _chaseRange = 5f;
    [SerializeField] private float _attackRange = 1.2f;
    [SerializeField] private float _stunTime = 0f;

    [Header("Ataque")]
    [SerializeField] private float _attackCooldown = 1f;
    [SerializeField] private float _damage = 10f;
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange = 0.5f;
    [SerializeField] LayerMask playerLayers;

    [Header("Detecci�n")]
    [SerializeField] private Transform _player;
    [SerializeField] private EnemyHealth _health;

    private Rigidbody2D _rb;
    private Animator _animator;
    private float _nextAttackTime = 0f;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _health = GetComponent<EnemyHealth>();

        if (_player == null)
            _player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (_player == null) return;
        if(_health.IsDead) return;

        if (_stunTime > 0f)
        {
            _stunTime -= Time.deltaTime;
            _animator.SetBool("IsRunning", false);
            return;
        }

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
            if (_animator != null)
            {
                _animator.SetTrigger("Attack");
            }

            Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);

            foreach(Collider2D player in hitPlayers)
            {
                player.GetComponent<IDamageable>().TakeDamage(_damage);
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

    void OnDrawGizmosSelected()
    {
        if(attackPoint == null) return;

        Gizmos.DrawWireSphere(attackPoint.position,attackRange);
    }

    public void Stun(float duration)
    {
        _stunTime = duration;
    }
}