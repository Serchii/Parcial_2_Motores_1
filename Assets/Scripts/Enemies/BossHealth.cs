using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class BossHealth : MonoBehaviour
{
    [Header("Vida")]
    public int maxHealth = 300;
    [SerializeField] private int currentHealth;

    [Header("Límites de fase (de mayor a menor). Deben tener 3 valores para 3 fases.")]
    [Tooltip("Ej: {200,100,0}")]
    public int[] phaseThresholds = new int[3] { 200, 100, 0 };

    private int currentPhase = 0;
    private bool isStunned = false;
    private Rigidbody2D rb;
    private Animator animator;

    [Header("Configuración de curación si falla el puzzle")]
    [Tooltip("Si falla el puzzle, el boss curará hasta este valor (por lo general el threshold anterior).")]
    public int healOnFailToThresholdOffset = 0; 

    [Header("Eventos")]
    public UnityEvent<int> OnHealthChanged;        
    public UnityEvent<int> OnPhaseReached;       
    public UnityEvent OnBossStunned;              
    public UnityEvent<bool> OnPuzzleResult;       
    public UnityEvent OnBossDefeated;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        currentHealth = maxHealth;

        if (phaseThresholds == null || phaseThresholds.Length < 3)
            phaseThresholds = new int[3] { maxHealth * 2 / 3, maxHealth / 3, 0 };

        Array.Sort(phaseThresholds);
        Array.Reverse(phaseThresholds);
    }

    private void Start()
    {
        OnHealthChanged?.Invoke(currentHealth);
    }


    public void TakeDamage(float amount, Vector2 knockbackDir, float knockbackForce, bool withKnockback)
    {
        if (isStunned) return; 

        currentHealth -= Mathf.RoundToInt(amount);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        OnHealthChanged?.Invoke(currentHealth);

        if (withKnockback && rb != null)
        {
            Vector2 dir = (knockbackDir.normalized);
            rb.AddForce(new Vector2(dir.x * knockbackForce, 0f), ForceMode2D.Impulse);
        }

        if (currentPhase < phaseThresholds.Length && currentHealth <= phaseThresholds[currentPhase])
        {

            isStunned = true;
            animator?.SetTrigger("Stunned");
            OnBossStunned?.Invoke();
            OnPhaseReached?.Invoke(currentPhase);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnBossDefeated?.Invoke();
        animator?.SetTrigger("Die");
        Destroy(gameObject, 5f); 
    }


    public void ResumeFightAfterPuzzle(bool puzzleSolved)
    {
        if (!isStunned) return;

        if (puzzleSolved)
        {
            currentPhase++;

            if (currentPhase >= phaseThresholds.Length)
            {
                currentHealth = 0;
                OnHealthChanged?.Invoke(currentHealth);
                Die();
                return;
            }

        }
        else
        {

            int prevIndex = Mathf.Max(currentPhase - 1, 0);
            int healTo = (currentPhase == 0) ? maxHealth : phaseThresholds[prevIndex];
            currentHealth = Mathf.Clamp(healTo, 0, maxHealth);
            OnHealthChanged?.Invoke(currentHealth);
        }

        isStunned = false;
        animator?.SetTrigger("Resume");
    }

    public int GetCurrentPhaseIndex() => currentPhase;
    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
    public bool IsStunned() => isStunned;
}
