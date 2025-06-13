using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //nuevo hit con LayerMask
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange = 0.5f;
    [SerializeField] LayerMask enemyLayers;
    [SerializeField] float attackDamage = 20f;
    [SerializeField] float attackCooldown = 1f;
    [SerializeField] float attackTimer;
    [SerializeField] float knockbackForce = 2;
    [SerializeField] GameObject hit;
    [SerializeField] float disableTime;

    [SerializeField] Animator animator;
    [SerializeField] AudioClip[] attackSounds;
    [SerializeField] AudioSource audioSource;
    [SerializeField] PlayerHealth playerHealth;

    private bool isHitting = false;

    public bool IsHitting => isHitting;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (playerHealth.IsAlive)
            HandleAttack();
    }

    private void HandleAttack()
    {
        if (!isHitting)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                StartAttack();
            }
        }
        else
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackCooldown)
            {
                isHitting = false;
                attackTimer = 0;
            }
        }

    }

    private void StartAttack()
    {
        isHitting = true;

        hit.SetActive(true);
        Invoke("DisableHit", disableTime);

        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                Vector2 knockbackDir = enemy.transform.position - transform.position;
                enemyHealth.TakeDamage(attackDamage, knockbackDir, knockbackForce);
            }
        }

        if (attackSounds.Length > 0 && audioSource != null)
        {
            int index = Random.Range(0, attackSounds.Length);
            audioSource.PlayOneShot(attackSounds[index]);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void DisableHit()
    {
        hit.SetActive(false);
    }

}
