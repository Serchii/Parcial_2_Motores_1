using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip JazzMusic;
    public AudioClip gameMusic;
    public AudioClip CombatMusic;

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

        if (sceneName == "Lv1_Classroom" || sceneName == "Lv2_Street_1" || sceneName == "Lv3_Street_2" || sceneName =="Lv6_Construccion")
            newClip = JazzMusic;
        else if (sceneName == "Lv4_House_2")
            newClip = gameMusic;
        else if (sceneName == "Lv5_EnemigosSueños" || sceneName == "Lv7_EnemigosConstruccion")
            newClip = CombatMusic;

        if (newClip != null && audioSource.clip != newClip)
        {
            audioSource.clip = newClip;
            audioSource.Play();
        }
    }
}