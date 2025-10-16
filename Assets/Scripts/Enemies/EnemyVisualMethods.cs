using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisualMethods : MonoBehaviour
{
    [SerializeField] protected EnemyBehaviour enemyBehaviour;

    protected virtual EnemyBehaviour Behaviour => enemyBehaviour;

    public void ActivateHit()
    {
        Behaviour.ActivateHit();
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
