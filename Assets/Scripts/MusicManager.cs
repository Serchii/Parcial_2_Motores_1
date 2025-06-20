using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip JazzMusic;
    public AudioClip gameMusic;
    public AudioClip CombatMusic;
    public AudioClip transitionMusic;
    public AudioClip startMusic;

    [SerializeField] string[] jazzScenes;
    [SerializeField] string[] gameScenes;
    [SerializeField] string[] combatScenes;
    [SerializeField] string[] transitionScenes;
    [SerializeField] string[] startScenes;

    private string currentScene;
    private static MusicManager instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        audioSource.volume = savedVolume;

        currentScene = SceneManager.GetActiveScene().name;
        PlayMusicForScene(currentScene);
        SceneManager.sceneLoaded += OnSceneLoaded;
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
        AudioClip newClip = null;

        if (jazzScenes.Contains(sceneName))
            newClip = JazzMusic;
        else if (gameScenes.Contains(sceneName))
            newClip = gameMusic;
        else if (combatScenes.Contains(sceneName))
            newClip = CombatMusic;
        else if (transitionScenes.Contains(sceneName))
            newClip = transitionMusic;
        else if (startScenes.Contains(sceneName))
            newClip = startMusic;


        if (newClip != null && audioSource.clip != newClip)
        {
            audioSource.clip = newClip;
            audioSource.Play();
        }
    }
    
    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }
}