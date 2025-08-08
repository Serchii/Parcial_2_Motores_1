using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip streetMusic;
    public AudioClip puzzleMusic;
    public AudioClip CombatMusic;
    public AudioClip transitionMusic;
    public AudioClip startMusic;

    [SerializeField] string[] streetScenes;
    [SerializeField] string[] puzzleScenes;
    [SerializeField] string[] combatScenes;
    [SerializeField] string[] transitionScenes;
    [SerializeField] string[] startScenes;
    [SerializeField] string[] stopMusic;

    private string currentScene;
    public static MusicManager instance;
    public static MusicManager Instance => instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded; // <- mover acá
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        currentScene = SceneManager.GetActiveScene().name;
        PlayMusicForScene(currentScene);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != currentScene)
        {
            currentScene = scene.name;
            PlayMusicForScene(currentScene);
        }
    }

    void PlayMusicForScene(string sceneName)
    {
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource is missing. Skipping music playback.");
            return;
        }

        AudioClip newClip = null;

        if (streetScenes.Contains(sceneName))
            newClip = streetMusic;
        else if (puzzleScenes.Contains(sceneName))
            newClip = puzzleMusic;
        else if (combatScenes.Contains(sceneName))
            newClip = CombatMusic;
        else if (transitionScenes.Contains(sceneName))
            newClip = transitionMusic;
        else if (startScenes.Contains(sceneName))
            newClip = startMusic;
        else if (stopMusic.Contains(sceneName))
        {
            audioSource.Stop();
            return;
        }

        ChangeAudioClip(newClip);
    }

    void ChangeAudioClip(AudioClip newClip)
    {
        if (newClip != null && audioSource.clip != newClip)
        {
            audioSource.clip = newClip;
            audioSource.Play();
        }
    }

    public void ChangeMusicWithFade(AudioClip newClip, float fadeDuration = 1f, float targetVolume = 1f, float lowerVolume = 0.2f)
    {
        if (newClip != null)
        {
            //StartCoroutine(FadeMusicRoutine(fadeDuration, targetVolume, lowerVolume, newClip));
            ChangeAudioClip(newClip);
        }
            
    }

    private IEnumerator FadeMusicRoutine(float duration, float targetVolume, float loweredVolume, AudioClip newClip)
    {
        float originalVolume = audioSource.volume;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            audioSource.volume = loweredVolume / originalVolume;
            yield return null;
        }

        yield return new WaitForSecondsRealtime(1f);

        ChangeAudioClip(newClip);

        t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(loweredVolume, targetVolume, t / duration);
            yield return null;
        }

        audioSource.volume = targetVolume;
    }
    
    private void OnDestroy()
    {
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}