using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] GameObject hitPrefab;
    [SerializeField] float attackDuration = 0.3f;    
    [SerializeField] float hitboxDuration = 0.1f;     
    [SerializeField] Animator animator;
    [SerializeField] AudioClip[] attackSounds;
    [SerializeField] AudioSource audioSource;
    [SerializeField] PlayerHealth playerHealth;

    private bool isHitting = false;
    private float attackTimer = 0f;

    public bool IsHitting => isHitting;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if(playerHealth.IsAlive)
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

            if (attackTimer >= hitboxDuration && hitPrefab.activeSelf)
            {
                hitPrefab.SetActive(false);
            }

            if (attackTimer >= attackDuration)
            {
                EndAttack();
            }
        }
    }

    private void StartAttack()
    {
        isHitting = true;
        attackTimer = 0f;
        hitPrefab.SetActive(true);
        
        Debug.Log("Atacando");

        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        if (attackSounds.Length > 0 && audioSource != null)
        {
            int index = Random.Range(0, attackSounds.Length);
            audioSource.PlayOneShot(attackSounds[index]);
        }
    }

    private void EndAttack()
    {
        isHitting = false;
        attackTimer = 0f;
        hitPrefab.SetActive(false);
    }
}
