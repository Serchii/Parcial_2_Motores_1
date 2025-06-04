using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;
    private Vector2 nextSpawnPosition;
    private bool waitingToPlacePlayer = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            GameSceneManager.OnSceneFullyLoaded += OnSceneFullyLoaded;
        }
        else Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GameSceneManager.OnSceneFullyLoaded -= OnSceneFullyLoaded;
    }

    public void LoadScene(string sceneName, Vector2 spawnPosition)
    {
        nextSpawnPosition = spawnPosition;
        waitingToPlacePlayer = true;
        StartCoroutine(GameSceneManager.Instance.LoadSceneWithTransitionRoutine(sceneName));
    }

    private void OnSceneFullyLoaded()
    {
        if (!waitingToPlacePlayer) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = nextSpawnPosition;
            Debug.Log("Escena cargada y jugador reubicado");
        }

        waitingToPlacePlayer = false;
    }
}
