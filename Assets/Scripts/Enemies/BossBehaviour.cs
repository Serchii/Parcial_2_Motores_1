using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossBehaviour : EnemyBehaviour
{
    [Header("Ataques del Jefe")]
    
    [SerializeField] private float bossAttackRange = 5;
    [SerializeField] private float shortAttackPushForce = 5;
    [SerializeField] protected Transform attackPoint2;
    [SerializeField] private float _attack1Cooldown = 2f;
    [SerializeField] private float _attack2Cooldown = 3f;
    [SerializeField] private float _attack3Cooldown = 4f;
    [SerializeField] private float _shortAttackRange = 1.5f; // Nueva distancia para ataques cortos

    private float _nextAttack1Time = 0f;
    private float _nextAttack2Time = 0f;
    private float _nextAttack3Time = 0f;

    private enum BossAttackState
    {
        None,
        Attack1,
        Attack2,
        Attack3
    }

    private BossAttackState currentBossAttack = BossAttackState.None;
    private BossAttackState _nextPlannedAttack = BossAttackState.None; // El ataque que el jefe planea usar
    private bool _isBossAttacking = false; // Para controlar si el jefe está en medio de un ataque especial

    // Sobrescribir HandleIdle para planificar el ataque al entrar en rango de persecución
    protected override void HandleIdle()
    {
        if (distanceToPlayer <= _chaseRange)
        {
            currentState = State.Move;
            setIdle = false;
            if (_nextPlannedAttack == BossAttackState.None)
            {
                ChooseAndPlanBossAttack(); // Planificar el ataque
            }
        }
        else
        {
            SetIdle();
            _nextPlannedAttack = BossAttackState.None; // Resetear ataque planeado si el jugador sale de rango
        }
    }

    // Sobrescribir el método HandleAttack para la lógica de ataque del jefe
    protected override void HandleAttack()
    {
        if (!_isBossAttacking && distanceToPlayer <= _attackRange) // Si no está atacando y hay un ataque planeado
        {
            ExecutePlannedAttack();
        }
        // Si _isBossAttacking es true, el ataque actual está en curso (ej. animación, proyectil)
        // y la lógica de ese ataque se encargará de resetear _isBossAttacking a false cuando termine.
    }

    private void ChooseAndPlanBossAttack()
    {
        List<BossAttackState> availableAttacks = new List<BossAttackState>();

        // Ataque 1: Ataque base de EnemyBehaviour, siempre disponible
        availableAttacks.Add(BossAttackState.Attack1);
        availableAttacks.Add(BossAttackState.Attack2);
        availableAttacks.Add(BossAttackState.Attack3);

        int randomIndex = Random.Range(0, availableAttacks.Count);
        _nextPlannedAttack = availableAttacks[randomIndex];
        Debug.Log($"Jefe ha planeado el ataque: {_nextPlannedAttack}");


        if (randomIndex > 1)
        {
            _attackRange = _shortAttackRange;
        }
        else
        {
            _attackRange = bossAttackRange;
        }
    }

    private void ExecutePlannedAttack()
    {
        if (_nextPlannedAttack == BossAttackState.None) return;

        currentBossAttack = _nextPlannedAttack; // Establecer el ataque actual al planeado

        switch (currentBossAttack)
        {
            case BossAttackState.Attack1:
                ExecuteAttack1();
                _nextAttack1Time = Time.time + _attack1Cooldown;
                break;
            case BossAttackState.Attack2:
                ExecuteAttack2();
                _nextAttack2Time = Time.time + _attack2Cooldown;
                break;
            case BossAttackState.Attack3:
                ExecuteAttack3();
                _nextAttack3Time = Time.time + _attack3Cooldown;
                break;
        }

        _nextPlannedAttack = BossAttackState.None;
    }

    // Este método se mantiene para ser llamado por ExecuteAttack1/2/3 si necesitan activar animaciones
    // o lógica de ataque base que use isAttacking y _animator.
    protected override void TryAttack()
    {
        // La lógica de cooldown y isAttacking ahora se gestiona en ExecutePlannedAttack
        // Este método solo se encarga de activar la animación y el estado de ataque
        isAttacking = true; // isAttacking de EnemyBehaviour
        if (_animator != null)
            _animator.SetTrigger($"Attack{(int)currentBossAttack}"); // Usar el ataque actual del jefe

        setIdle = false;
    }

    void TryShortAttack(float nextAttackTimeRef, float attackCooldown, int attackExecuted)
    {
        // La lógica de cooldown y isAttacking ahora se gestiona en ExecutePlannedAttack
        // Este método solo se encarga de activar la animación y el estado de ataque
        isAttacking = true; // isAttacking de EnemyBehaviour
        if (_animator != null)
            _animator.SetTrigger($"Attack{attackExecuted}");

        setIdle = false;
    }

    private void ExecuteAttack1()
    {
        Debug.Log("Jefe ejecutando Ataque 1 (Ataque base)");
        // Llama a la lógica de ataque base de EnemyBehaviour
        TryAttack(); 
        // EndBossAttack() se llamará desde la animación.
    }

    private void ExecuteAttack2()
    {
        Debug.Log("Jefe ejecutando Ataque 2");
        TryShortAttack(_nextAttack2Time, _attack2Cooldown, 2);
        // EndBossAttack() se llamará desde la animación.
    }

    private void ExecuteAttack3()
    {
        Debug.Log("Jefe ejecutando Ataque 3");
        TryShortAttack(_nextAttack3Time, _attack3Cooldown, 3);
    }

    // Método para ser llamado cuando un ataque del jefe ha terminado
    public void EndBossAttack()
    {
        _isBossAttacking = false;
        isAttacking = false;
        currentBossAttack = BossAttackState.None;
        currentState = State.Idle; // Volver al estado Idle después de cada ataque
        _nextPlannedAttack = BossAttackState.None; // Resetear el ataque planeado para que se elija uno nuevo
        Debug.Log("Jefe: Ataque terminado. Volviendo a Idle y planificando nuevo ataque.");
    }

    public void ActivateShortHit()
    {
        attackPoint2.gameObject.SetActive(true);

        float direction = transform.localScale.x > 0 ? 1f : -1f;

        _rb.velocity = new Vector2(direction * shortAttackPushForce, _rb.velocity.y);
    }

    public override void DeactivateHit()
    {
        attackPoint.gameObject.SetActive(false);
        attackPoint2.gameObject.SetActive(false);
    }
}
