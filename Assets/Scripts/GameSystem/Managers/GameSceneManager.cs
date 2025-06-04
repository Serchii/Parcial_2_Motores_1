using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance { get; private set; }

    public static event Action OnSceneFullyLoaded;

    private bool isChanging = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadSceneWithTransition(string sceneName)
    {
        if (isChanging) return;

        UITransitionManager transition = GameObject.FindGameObjectWithTag("UITransitionManager")?.GetComponent<UITransitionManager>();

        if (transition != null)
        {
            isChanging = true;
            StartCoroutine(HandleSceneLoad(transition, sceneName));
        }
        else
        {
            Debug.LogWarning("No se encontró un objeto con tag UITransitionManager en la escena.");
            SceneManager.LoadScene(sceneName);
            Time.timeScale = 1f;
            OnSceneFullyLoaded?.Invoke();
        }
    }

    public IEnumerator LoadSceneWithTransitionRoutine(string sceneName)
    {
        if (isChanging) yield break;

        UITransitionManager transition = GameObject.FindGameObjectWithTag("UITransitionManager")?.GetComponent<UITransitionManager>();

        if (transition != null)
        {
            isChanging = true;
            yield return StartCoroutine(HandleSceneLoad(transition, sceneName));
        }
        else
        {
            Debug.LogWarning("No se encontró un objeto con tag UITransitionManager en la escena.");
            SceneManager.LoadScene(sceneName);
            Time.timeScale = 1f;
            OnSceneFullyLoaded?.Invoke();
        }
    }

    private IEnumerator HandleSceneLoad(UITransitionManager transition, string sceneName)
    {
        yield return transition.PlayTransition();

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        transition.ShowLoadPanel();

        while (!asyncOperation.isDone)
        {
            transition.UpdateLoadbar(asyncOperation.progress / 0.9f);
            yield return null;
        }

        yield return new WaitForSeconds(0.1f); // opcional: pequeño delay para estabilidad

        isChanging = false;
        Time.timeScale = 1f;
        OnSceneFullyLoaded?.Invoke();
    }
}
