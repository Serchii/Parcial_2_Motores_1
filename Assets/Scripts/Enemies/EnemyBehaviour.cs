using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _chaseRange = 5f;
    [SerializeField] private float _attackRange = 1.2f;
    [SerializeField] private float _stunTime = 0f;

    [Header("Salto")]
    [SerializeField] private float _jumpForce = 7f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float jumpCheckDistance = 1.2f;

    [Header("Salto con Delay")]
    [SerializeField] private float jumpDelay = 0.5f;
    private bool isPreparingToJump = false;

    [Header("Ataque")]
    [SerializeField] private float _attackCooldown = 1f;
    [SerializeField] private float _damage = 10f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private LayerMask playerLayers;

    [Header("Detección")]
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
        if (_player == null || _health.IsDead) return;

        if (_stunTime > 0f)
        {
            _stunTime -= Time.deltaTime;
            _animator.SetBool("IsRunning", false);
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, _player.position);

        if (distanceToPlayer <= _attackRange)
        {
            _rb.velocity = new Vector2(0f, _rb.velocity.y);
            _animator.SetBool("IsRunning", false);
            TryAttack();
        }
        else if (distanceToPlayer <= _chaseRange)
        {
            MoveTowardsPlayer();
        }
        else
        {
            if (IsGrounded())
                _rb.velocity = new Vector2(0f, 0f);
            else
                _rb.velocity = new Vector2(0f, _rb.velocity.y);

            _animator.SetBool("IsRunning", false);
        }

        FlipSprite();
    }

    private void MoveTowardsPlayer()
    {
        if (_player == null) return;

        Vector2 direction = (_player.position - transform.position).normalized;

        _rb.velocity = new Vector2(direction.x * _moveSpeed, _rb.velocity.y);
        _animator.SetBool("IsRunning", true);

        float verticalDifference = _player.position.y - transform.position.y;

        if (verticalDifference > jumpCheckDistance && IsGrounded() && !isPreparingToJump)
        {
            StartCoroutine(DelayedJump());
        }
    }

    private IEnumerator DelayedJump()
    {
        isPreparingToJump = true;
        yield return new WaitForSeconds(jumpDelay);

        float verticalDifference = _player.position.y - transform.position.y;

        if (IsGrounded() && verticalDifference > jumpCheckDistance)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
        }

        isPreparingToJump = false;
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void TryAttack()
    {
        if (Time.time >= _nextAttackTime)
        {
            _nextAttackTime = Time.time + _attackCooldown;

            if (_animator != null)
                _animator.SetTrigger("Attack");

            Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);

            foreach (Collider2D player in hitPlayers)
            {
                player.GetComponent<IDamageable>()?.TakeDamage(_damage);
            }
        }
    }

    private void FlipSprite()
    {
        if (_player == null) return;

        Vector3 scale = transform.localScale;

        if (_player.position.x < transform.position.x)
        {
            if (scale.x < 0)
                scale.x *= -1;
        }
        else
        {
            if (scale.x > 0)
                scale.x *= -1;
        }

        transform.localScale = scale;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);

        if (groundCheck != null)
            Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
    }

    public void Stun(float duration)
    {
        _stunTime = duration;
    }
}