using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualMethods : MonoBehaviour
{
    [SerializeField] PlayerAttack playerAttack;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip stepClip;

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

    public void PlaySFXSteps()
    {
        audioSource.clip = stepClip;
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.Play();
    }

    public void CreateDust()
    {
        playerAttack.GetComponent<PlayerMovement>().CreateDust();
    }
}
