using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;

    private float moveInput;
    [SerializeField] private bool isGrounded;

    [Header("Ground Check")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] GameObject currentOneWayPlatform;
    [SerializeField] float disableCollisionTime = 0.25f;

    private bool isKnockedBack = false;
    [SerializeField] public float knockbackDuration = 0.2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        SetAnimator(moveInput, !isGrounded);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (Input.GetButtonDown("Down") && isGrounded)
        {
            if (currentOneWayPlatform != null)
            {
                StartCoroutine(DisableCollision());
            }
        }

        FlipSprite();
    }

    void SetAnimator(float run, bool jump)
    {
        animator.SetFloat("Run", Mathf.Abs(run));
        animator.SetBool("Jump", jump);
    }

    void FixedUpdate()
    {
        if (!isKnockedBack)
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }
    }

    private void FlipSprite()
    {
        if (moveInput != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(moveInput);
            transform.localScale = scale;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = col.gameObject;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = null;
        }
    }

    IEnumerator DisableCollision()
    {
        Collider2D platformCollider = currentOneWayPlatform.GetComponent<Collider2D>();
        Collider2D playerCollider = GetComponent<Collider2D>();

        if (platformCollider == null)
        {
            Debug.LogWarning("La plataforma no tiene ningún Collider2D.");
            yield break;
        }

        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(disableCollisionTime);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }

    public void TriggerKnockback(float duration)
    {
        isKnockedBack = true;
        Invoke(nameof(EndKnockback), duration);
    }

    private void EndKnockback()
    {
        isKnockedBack = false;
    }
}
