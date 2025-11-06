using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Localization.Settings;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _chaseRange = 5f;
    [SerializeField] private float _attackRange = 1.2f;
    [SerializeField] private float _stunTime = 0f;
    [SerializeField] bool setIdle = false;

    [Header("Salto")]
    [SerializeField] private float _jumpForce = 7f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float jumpCheckDistance = 1.2f;

    [Header("Salto con Delay")]
    [SerializeField] private float jumpDelay = 0.5f;
    private bool isPreparingToJump = false;

    [Header("Sonidos")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource attackSource;
    [SerializeField] AudioClip hitClip;
    [SerializeField] AudioClip anticipationClip;
    [SerializeField] AudioClip attackClip;

    [Header("Ataque")]
    [SerializeField] private float _attackCooldown = 1f;
    [SerializeField] private float attackPushForce = 3f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask playerLayers;
    private bool isAttacking = false;

    public bool IsAttacking => isAttacking;

    [Header("Detección")]
    [SerializeField] private Transform _player;
    [SerializeField] private EnemyHealth _health;
    float distanceToPlayer;

    [Header("Texto de enemigo")]
    [SerializeField] List<string> enemyPhrasesKeys;
    [SerializeField] bool enemyTalk = false;
    [SerializeField] string tableName = "Enemy Phrases";
    [SerializeField] private GameObject enemyTextPrefab; // Reference to the EnemyTextDisplay prefab

    private Rigidbody2D _rb;
    [SerializeField] Animator _animator;
    private float _nextAttackTime = 0f;

    [SerializeField] State currentState;
    private enum State
    {
        Idle,
        Move,
        Attack
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _health = GetComponent<EnemyHealth>();

        if (_player == null)
            _player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (_player == null || _health.IsDead)
        {
            Debug.Log("EnemyBehaviour: Player es nulo o enemigo está muerto. Saliendo de Update.");
            return;
        }

        if (GameStateManager.Instance.StateMachine.CurrentState != GameStateManager.Instance.Gameplay)
        {
            SetIdle();
            return;
        }

        if (_stunTime > 0f)
            {
                _stunTime -= Time.deltaTime;
                _animator.SetBool("Hurt", false);
                return;
            }

        distanceToPlayer = Vector2.Distance(transform.position, _player.position);

        switch (currentState)
        {
            case State.Idle:
                HandleIdle();
                break;
            case State.Move:
                HandleMove();
                break;
            case State.Attack:
                HandleAttack();
                break;
        }

        if (!isAttacking)
            FlipSprite();
    }

    void HandleIdle()
    {
        if (distanceToPlayer <= _chaseRange)
        {
            currentState = State.Move;
            setIdle = false;
        }
        else
        {
            SetIdle();
        }
    }

    void SetIdle()
    {
        if (IsGrounded())
            if (!setIdle)
            {
                _rb.velocity = new Vector2(0f, 0f);
                setIdle = true;
            }
            else
                _rb.velocity = new Vector2(0f, _rb.velocity.y);

        _animator.SetBool("IsRunning", false);
    }

    void HandleMove()
    {
        MoveTowardsPlayer();
    }

    void HandleAttack()
    {
        if (!isAttacking)
            if (!setIdle)
            {
                _rb.velocity = new Vector2(0f, _rb.velocity.y);
                setIdle = true;
            }
        _animator.SetBool("IsRunning", false);
        TryAttack();
    }

    private void MoveTowardsPlayer()
    {
        if (_player == null)
        {
            Debug.Log("EnemyBehaviour: Player es nulo en MoveTowardsPlayer. Saliendo.");
            return;
        }

        if (enemyTalk && distanceToPlayer <= _chaseRange && distanceToPlayer > _attackRange)
        {
            ShowEnemyPhrase();
            enemyTalk = false;
        }

        if (distanceToPlayer <= _attackRange)
        {
            currentState = State.Attack;
        }

        Vector2 direction = (_player.position - transform.position).normalized;

        _rb.velocity = new Vector2(direction.x * _moveSpeed, _rb.velocity.y);
        _animator.SetBool("IsRunning", true);

        float verticalDifference = _player.position.y - transform.position.y;

        if (verticalDifference > jumpCheckDistance && IsGrounded() && !isPreparingToJump)
        {
            StartCoroutine(DelayedJump());
        }

        if (distanceToPlayer > _chaseRange)
        {
            currentState = State.Idle;
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

            isAttacking = true;

            if (_animator != null)
                _animator.SetTrigger("Attack");

            setIdle = false;
        }
    }

    public void ActivateHit()
    {
        attackPoint.gameObject.SetActive(true);

        float direction = transform.localScale.x > 0 ? 1f : -1f;

        _rb.velocity = new Vector2(direction * attackPushForce, _rb.velocity.y);
    }

    public void DeactivateHit()
    {
        attackPoint.gameObject.SetActive(false);
    }

    public void EndAttack()
    {
        isAttacking = false;
        currentState = State.Idle;
    }

    private void FlipSprite()
    {
        if (_player == null) return;

        Vector3 scale = transform.localScale;

        if (_player.position.x < transform.position.x)
        {
            if (scale.x > 0)
                scale.x *= -1;
        }
        else
        {
            if (scale.x < 0)
                scale.x *= -1;
        }

        transform.localScale = scale;
    }

    public void Stun(float duration)
    {
        _stunTime = duration;
    }

    public void PlaySFXHit()
    {
        audioSource.clip = hitClip;
        PlaySFX(audioSource);
    }

    void PlaySFX(AudioSource audioSource)
    {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.Play();
    }

    public void PlaySFXAttack()
    {
        attackSource.clip = attackClip;
        PlaySFX(attackSource);
    }

    public void PlaySFXAnticipation()
    {
        attackSource.clip = anticipationClip;
        PlaySFX(attackSource);
    }

    private void ShowEnemyPhrase()
    {
        if (enemyPhrasesKeys == null || enemyPhrasesKeys.Count == 0)
        {
            return;
        }
        if (enemyTextPrefab == null)
        {
            return;
        }

        int randomIndex = Random.Range(0, enemyPhrasesKeys.Count);
        string key = enemyPhrasesKeys[randomIndex];

        // Get localized string
        var localizedString = LocalizationSettings.StringDatabase.GetLocalizedString(tableName, key);
        
        // Find the main Canvas in the scene
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("EnemyBehaviour: No se encontró ningún Canvas en la escena. No se puede mostrar el texto del enemigo.");
            return;
        }

        // Instantiate the text prefab as a child of the Canvas
        GameObject textInstance = Instantiate(enemyTextPrefab, canvas.transform);
        EnemyTextDisplay textDisplay = textInstance.GetComponent<EnemyTextDisplay>();

        if (textDisplay != null)
        {
            textDisplay.SetTarget(transform); // Set the enemy as the target to follow
            textDisplay.ShowText(localizedString);
        }
        else
        {
            Destroy(textInstance);
        }
    }
}
