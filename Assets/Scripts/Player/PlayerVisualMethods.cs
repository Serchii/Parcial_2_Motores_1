using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualMethods : MonoBehaviour
{
    [SerializeField] PlayerAttack playerAttack;

    public void StartCombo()
    {
        playerAttack.StartCombo();
        Debug.Log("Visual: Start Combo");
    }

    public void FinishAttack()
    {
        playerAttack.FinishAttack();
        Debug.Log("Visual: FinishAttack");
    }

    public void ExecuteAttack()
    {
        playerAttack.ExecuteAttack();
        Debug.Log("Visual: ExecuteAttack");
    }
}
