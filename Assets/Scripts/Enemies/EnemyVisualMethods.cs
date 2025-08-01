using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisualMethods : MonoBehaviour
{
    [SerializeField] EnemyBehaviour enemyBehaviour;

    public void ActivateHit()
    {
        enemyBehaviour.ActivateHit();
    }   

    public void DeactivateHit()
    {
        enemyBehaviour.DeactivateHit();
    }

    public void EndAttack()
    {
        enemyBehaviour.EndAttack();
    }
}
