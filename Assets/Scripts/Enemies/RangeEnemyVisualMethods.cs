using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyVisualMethods : MonoBehaviour
{
    [SerializeField] protected RangeEnemyBehaviour  enemyBehaviour;

    protected virtual RangeEnemyBehaviour  Behaviour => enemyBehaviour;

    public void Shoot()
    {
        Behaviour.Shoot();
    }

    public void DeactivateHit()
    {
        Behaviour.DeactivateHit();
    }

    public void EndAttack()
    {
        Behaviour.EndAttack();
    }

    public void AnticipationSFX()
    {
        Behaviour.PlaySFXAnticipation();
    }

    public void AttackSFX()
    {
        Behaviour.PlaySFXAttack();
    }
}
