using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    [Header("Ground Check")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] private List<GameObject> currentOneWayPlatforms = new List<GameObject>();
    [SerializeField] private List<GameObject> temporarilyIgnoredPlatforms = new List<GameObject>();
    [SerializeField] float disableCollisionTime = 0.25f;

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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        playerAttack = GetComponent<PlayerAttack>();
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (!isKnockedBack && canMove)
            SetAnimator(moveInput, !isGrounded);

        if (isGrounded)
        {
            coyoteTimer = coyoteTime;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && (isGrounded || coyoteTimer > 0) && canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            canJump = false;
            Invoke("ActivateJump", 0.5f);
            coyoteTimer = 0;
        }

        if (Input.GetButtonDown("Down") && isGrounded)
        {
            StartCoroutine(DisableCollision());
        }

        FlipSprite();
    }

    void SetAnimator(float run, bool jump)
    {
        animator.SetFloat("Run", Mathf.Abs(run));
        animator.SetBool("Jump", jump);
        Debug.Log($"Run: {run}, Jump: {jump}");
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
        Invoke(nameof(EndKnockback), duration);
    }

    private void EndKnockback()
    {
        isKnockedBack = false;
        playerAttack.FinishAttack();
        Debug.Log("EndKnockbackACAAAAAA");
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
}
