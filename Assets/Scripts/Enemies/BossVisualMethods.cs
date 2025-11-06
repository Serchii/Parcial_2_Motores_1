using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossVisualMethods : MonoBehaviour
{
    [SerializeField] protected BossBehaviour enemyBehaviour;

    protected virtual BossBehaviour Behaviour => enemyBehaviour;

    public void ActivateHit()
    {
        Behaviour.ActivateHit();
    }

    public void ActivateShortHit()
    {
        Behaviour.ActivateShortHit();
    }

    public void DeactivateHit()
    {
        Behaviour.DeactivateHit();
    }

    public void EndAttack()
    {
        Behaviour.EndBossAttack();
    }

    public void AnticipationSFX()
    {
        Behaviour.PlaySFXAnticipation();
    }

    public void AttackSFX()
    {
        Behaviour.PlaySFXAttack();
    }

    public void FlipSprite()
    {
        Behaviour.FlipSprite();
    }
}
