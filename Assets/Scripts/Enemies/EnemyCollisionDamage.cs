using UnityEngine;

public class EnemyCollisionDamage : MonoBehaviour
{
    [SerializeField] private float damageAmount = 1f;
    [SerializeField] private float knockbackForce = 15f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                // Dirección lateral forzada
                float directionX = Mathf.Sign(collision.transform.position.x - transform.position.x);
                Vector2 knockbackDirection = new Vector2(directionX, 0f);

                playerHealth.TakeDamage(damageAmount);
                playerHealth.ApplyKnockback(knockbackDirection, knockbackForce);
            }
        }
    }
}