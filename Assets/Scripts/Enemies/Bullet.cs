using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 100f;
    [SerializeField] private float damageAmount = 20f;
    [SerializeField] private float knockbackForce = 15f;
    [SerializeField] Transform startPos;
    [SerializeField] float hitstopDuration = .2f;

    void Start()
    {
        startPos = transform;
    }
    
    void Update()
    {
        // Movimiento lineal constante
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnBecameInvisible()
    {
        // Se destruye cuando sale de cámara
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) 
            return;
        else
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth.Invincible)
            {
                return;
            }
            else
            {
                ShakeEffect shake = playerHealth.GetComponentInChildren<ShakeEffect>();

                if (playerHealth != null)
                {
                    float directionX = Mathf.Sign(collision.transform.position.x - startPos.position.x);
                    Vector2 knockbackDirection = new Vector2(directionX, 0f);
                    if (!playerHealth.Invincible)
                    {
                        //collision.GetComponent<EnemyBehaviour>().PlaySFXHit();

                        if (shake != null)
                            shake.Shake();
                            
                        playerHealth.ApplyKnockback(knockbackDirection, knockbackForce);
                        playerHealth.TakeDamage(damageAmount);
                        collision.GetComponent<PlayerAttack>().PlaySFXShot();
                        if (HitstopManager.Instance != null)
                            HitstopManager.Instance.DoHitstop(hitstopDuration);
                    }
                }
            }
        }
        // Podés manejar colisiones acá
        Destroy(gameObject);
    }
}