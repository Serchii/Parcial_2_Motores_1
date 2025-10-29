using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;
    [SerializeField] bool canMove = true;

    private float moveInput;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool canJump = true;

    [Header("Particles System")]
    [SerializeField] ParticleSystem dustPS;

    [Header("Ground Check")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] private List<GameObject> currentOneWayPlatforms = new List<GameObject>();
    [SerializeField] private List<GameObject> temporarilyIgnoredPlatforms = new List<GameObject>();
    [SerializeField] float disableCollisionTime = 0.25f;
    [SerializeField] GameObject dustEffect;

    [Header("Dash")]
    [SerializeField] float dashSpeed = 30f;
    [SerializeField] float dashTime = 0.2f;
    [SerializeField] float dashCooldown = 1f;
    [SerializeField] bool canDash = true;
    [SerializeField] bool isDashing = false;
    [SerializeField] TrailRenderer trailRenderer;
    [SerializeField] PlayerHealth playerHealth;

    [Header("Coyote Time")]
    [SerializeField] float coyoteTime = 0.2f;
    [SerializeField] float coyoteTimer;

    [Header("Knockback")]
    [SerializeField] bool isKnockedBack = false;
    [SerializeField] public float knockbackDuration = 0.2f;
    [SerializeField] private float attackFrictionFactor = 0.8f;
    [SerializeField] PlayerAttack playerAttack;

    public bool IsKnockedBack => isKnockedBack;
    public bool CanMove => canMove;
    public bool IsDashing => isDashing;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (playerHealth == null)
            playerHealth = GetComponent<PlayerHealth>();

        playerAttack = GetComponent<PlayerAttack>();

        InputManager.Instance.OnJumpPressed += Jump;
        InputManager.Instance.OnDashPressed += Dash;
        InputManager.Instance.OnDownPressed += FallDownPlatforms;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnJumpPressed -= Jump;
        InputManager.Instance.OnDashPressed -= Dash;
        InputManager.Instance.OnDownPressed -= FallDownPlatforms;
    }

    void Update()
    {
        if (!isKnockedBack && canMove && !isDashing)
        {
            moveInput = InputManager.Instance.Horizontal;
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }

        SetAnimator(moveInput, !isGrounded);

        if (isGrounded)
        {
            coyoteTimer = coyoteTime;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }

        FlipSprite();
    }

    void Dash()
    {
        if (canDash && !isKnockedBack)
        {
            StartCoroutine(SetDash());
        }
    }

    void SetAnimator(float run, bool jump)
    {
        animator.SetFloat("Run", Mathf.Abs(run));
        animator.SetBool("Jump", jump);
    }

    void Jump()
    {
        if ((isGrounded || coyoteTimer > 0) && canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            canJump = false;
            Invoke("ActivateJump", 0.5f);
            CreateDust();
            coyoteTimer = 0;
        }
    }

    IEnumerator SetDash()
    {
        canDash = false;
        isDashing = true;
        SetCanMove(false);
        rb.velocity = new Vector2(transform.localScale.x * dashSpeed, rb.velocity.y);
        trailRenderer.emitting = true;
        animator.SetTrigger("Dash");
        playerHealth.EnableInvincible();

        Debug.Log(transform.localScale.x * dashSpeed);

        yield return new WaitForSeconds(dashTime);

        isDashing = false;
        SetCanMove(true);
        trailRenderer.emitting = false;
        playerHealth.DisableInvincible();

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }

    void FallDownPlatforms()
    {
        if (isGrounded)
        {
            StartCoroutine(DisableCollision());
        }
    }

    void FixedUpdate()
    {
        if (!isKnockedBack && canMove)
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }
        else if (isKnockedBack)
        {
            ApplyAttackFriction();
        }
    }

    void ApplyAttackFriction()
    {
        rb.velocity = new Vector2(rb.velocity.x * attackFrictionFactor, rb.velocity.y);
    }

    private void FlipSprite()
    {
        if (moveInput != 0 && !isKnockedBack && canMove)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(moveInput);
            transform.localScale = scale;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("OneWayPlatform") && !currentOneWayPlatforms.Contains(col.gameObject))
        {
            currentOneWayPlatforms.Add(col.gameObject);
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("OneWayPlatform"))
        {
            // Solo remové si NO está en la lista de plataformas ignoradas manualmente
            if (!temporarilyIgnoredPlatforms.Contains(col.gameObject))
            {
                currentOneWayPlatforms.Remove(col.gameObject);
            }
        }
    }

    IEnumerator DisableCollision()
    {
        Collider2D playerCollider = GetComponent<Collider2D>();
        temporarilyIgnoredPlatforms.Clear();

        foreach (GameObject platform in currentOneWayPlatforms)
        {
            Collider2D platformCollider = platform.GetComponent<Collider2D>();
            if (platformCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, platformCollider);
                temporarilyIgnoredPlatforms.Add(platform); // Agregamos a la lista temporal
            }
        }

        yield return new WaitForSeconds(disableCollisionTime);

        foreach (GameObject platform in temporarilyIgnoredPlatforms)
        {
            Collider2D platformCollider = platform.GetComponent<Collider2D>();
            if (platformCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
            }
        }

        temporarilyIgnoredPlatforms.Clear(); // Limpiamos al final
    }

    public void TriggerKnockback(float duration)
    {
        isKnockedBack = true;
        moveInput = 0;
        Invoke(nameof(EndKnockback), duration);
    }

    private void EndKnockback()
    {
        isKnockedBack = false;
        playerAttack.FinishAttack();
    }

    private void ActivateJump()
    {
        canJump = true;
    }

    public void SetCanMove(bool value)
    {
        canMove = value;

        if (!value)
            moveInput = 0;

        rb.velocity = new Vector2(moveInput, rb.velocity.y);
        FlipSprite();
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    public void CreateDust()
    {
        if (dustEffect != null)
            Instantiate(dustEffect, groundCheck.transform.position, Quaternion.identity);
    }
}
