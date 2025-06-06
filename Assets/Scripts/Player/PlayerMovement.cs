using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] Rigidbody2D rb;

    private float moveInput;
    private bool isGrounded;
    private bool isFacingRight = true; // <-- NUEVA variable para controlar la dirección

    [Header("Ground Check")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] GameObject currentOneWayPlatform;
    [SerializeField] float disableCollisionTime = 0.25f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Salto
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // Bajar plataforma
        if (Input.GetButtonDown("Down") && isGrounded)
        {
            if (currentOneWayPlatform != null)
            {
                StartCoroutine(DisableCollision());
            }
        }

        Flip(); // <-- Llamar para girar el personaje
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    void Flip()
    {
        // Girar solo si el jugador cambia de dirección
        if ((moveInput > 0 && !isFacingRight) || (moveInput < 0 && isFacingRight))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
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
        EdgeCollider2D currentCollider = currentOneWayPlatform.GetComponent<EdgeCollider2D>();
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), currentCollider);
        yield return new WaitForSeconds(disableCollisionTime);
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), currentCollider, false);
    }
}