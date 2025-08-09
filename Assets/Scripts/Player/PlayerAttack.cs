using System.ComponentModel;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //nuevo hit con LayerMask
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange = 0.5f;
    [SerializeField] LayerMask enemyLayers;
    [SerializeField] int combo;
    [SerializeField] float attackDamage = 20f;
    [Header("Knockback Variables")]
    [SerializeField] float knockbackForce;
    [SerializeField] float baseForce = 5;    
    [SerializeField] float additionalForce;
    [SerializeField] GameObject hit;

    [Header("Hitstop Variables")]
    [SerializeField] float hitstopDuration = 0.1f;
    [SerializeField] float softHitstopDuration = 0.1f;
    [SerializeField] float hardHitstopDuration = 0.2f;

    [Header("Attack Buffer")]
    [SerializeField] float attackBufferTime = 0.1f;
    [SerializeField] float attackBufferTimer;
    

    [SerializeField] Animator animator;
    [SerializeField] AudioClip hitClip;
    [SerializeField] AudioSource audioSource;
    [SerializeField] PlayerHealth playerHealth;

    [SerializeField] Rigidbody2D rb;
    [SerializeField] float attackPushForce = 6f;
    [SerializeField] private float attackFrictionFactor = 0.8f;
    [SerializeField] bool attackWithKnockback = false;

    [SerializeField] bool isHitting = false;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] bool canAttack = true;
    public bool IsHitting => isHitting;
    public bool CanAttack => canAttack;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerMovement = GetComponent<PlayerMovement>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        rb = GetComponent<Rigidbody2D>();
        ApplyUpgrades();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            attackBufferTimer = attackBufferTime;
        }
        else
        {
            attackBufferTimer -= Time.deltaTime;
        }

        if (playerHealth.IsAlive && !playerMovement.IsKnockedBack && canAttack)
            Combo();
    }

    void FixedUpdate()
    {
        if (isHitting)
        {
            ApplyAttackFriction();
        }
    }

    void ApplyAttackFriction()
    {
        rb.velocity = new Vector2(rb.velocity.x * attackFrictionFactor, rb.velocity.y);
    }
    void Combo()
    {
        if (!isHitting)
        {
            if (attackBufferTimer > 0)
            {
                PlayerCanMove(true); //Esto lo hago para que en caso de que en medio del combo quiera cambiar de direccion pueda hacerlo solo al seguir con el siguiente golpe
                
                isHitting = true;

                PlayerCanMove(false);

                animator.SetTrigger("Attack" + (combo + 1));

                //Genero el knockback solo en el ultimo golpe
                attackWithKnockback = combo >= 2;
                if (attackWithKnockback)
                {
                    hitstopDuration = hardHitstopDuration;
                    knockbackForce = baseForce + additionalForce;
                }
                else
                {
                    hitstopDuration = softHitstopDuration;
                    knockbackForce = baseForce;
                }

                float direction = transform.localScale.x > 0 ? 1f : -1f;

                rb.AddForce(new Vector2(direction * attackPushForce, 0f), ForceMode2D.Impulse);

                //ExecuteAttack();
            }
        }
    }

    public void ExecuteAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            ShakeEffect shake = enemy.GetComponentInChildren<ShakeEffect>();
            PlaySFXHit();

            Debug.Log("Shake: " + shake);

            if (enemyHealth != null)
            {
                Vector2 knockbackDir = enemy.transform.position - transform.position;
                if (HitstopManager.Instance != null)
                    HitstopManager.Instance.DoHitstop(hitstopDuration);

                if (shake != null)
                    shake.Shake();

                enemyHealth.TakeDamage(attackDamage, knockbackDir, knockbackForce, attackWithKnockback);
            }
        }
    }


    void PlayerCanMove(bool value)
    {
        playerMovement.SetCanMove(value);
    }

    public void StartCombo()
    {
        isHitting = false;
        if (combo < 3)
        {
            combo++;
        }
    }

    public void FinishAttack()
    {
        isHitting = false;
        combo = 0;
        PlayerCanMove(true);
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

    public void SetAttackDamage(float amount)
    {
        attackDamage = amount;
    }

    public void SetCanAttack(bool value)
    {
        canAttack = value;
    }

    void ApplyUpgrades()
    {
        if (PlayerInventory.Instance != null)
        {
            if (PlayerInventory.Instance.HasItem(ItemID.HammerUltimate))
                SetAttackDamage(30f);
            else if (PlayerInventory.Instance.HasItem(ItemID.HammerImproved))
                SetAttackDamage(20f);
            else
                SetAttackDamage(10f);
        }
    }

    public void PlaySFXHit()
    {
        audioSource.clip = hitClip;
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.Play();
    }
}
