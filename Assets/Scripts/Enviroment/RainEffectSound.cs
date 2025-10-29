using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainEffectSound : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        GameStateManager.Instance.Paused.OnPausedGame += PauseSFX;
        GameStateManager.Instance.Paused.OnResumedGame += PlaySFX;
    }

    void OnDisable()
    {
        GameStateManager.Instance.Paused.OnPausedGame -= PauseSFX;
        GameStateManager.Instance.Paused.OnResumedGame -= PlaySFX;
    }

    void PlaySFX()
    {
        audioSource.UnPause();
    }
    
    void PauseSFX()
    {
        audioSource.Pause();
    }

}
