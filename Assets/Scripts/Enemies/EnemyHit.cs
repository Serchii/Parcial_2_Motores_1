using Unity.VisualScripting;
using UnityEngine;

public class EnemyHit : BaseHit
{
    [SerializeField] private float damageAmount = 1f;
    [SerializeField] private float knockbackForce = 15f;
    [SerializeField] Transform enemy;
    [SerializeField] float hitstopDuration = .1f;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            ShakeEffect shake = playerHealth.GetComponentInChildren<ShakeEffect>();

            if (playerHealth != null)
            {
                float directionX = Mathf.Sign(collision.transform.position.x - enemy.position.x);
                Vector2 knockbackDirection = new Vector2(directionX, 0f);
                if (!playerHealth.Invincible)
                {
                    enemy.GetComponent<EnemyBehaviour>().PlaySFXHit();

                    if (shake != null)
                        shake.Shake();

                    playerHealth.ApplyKnockback(knockbackDirection, knockbackForce);
                    playerHealth.TakeDamage(damageAmount);
                    if (HitstopManager.Instance != null)
                        HitstopManager.Instance.DoHitstop(hitstopDuration);
                }
            }
        }
    }
    
    public void SetDamageAmount(float amount)
    {
        damageAmount = amount;
    }
}