using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private bool gameOver = false;
    private bool youWon = false;

    public bool IsGameOver() => gameOver || youWon;

    public static event Action<bool, string> OnGameEnded;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Lo hace persistente entre escenas
            GameSceneManager.OnSceneFullyLoaded += ResetGameState;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            GameSceneManager.OnSceneFullyLoaded -= ResetGameState;
        }
    }

    public void PlayerDied()
    {
        if (gameOver) return;
        gameOver = true;

        OnGameEnded?.Invoke(false, "YOU ARE DEAD");
    }

    public void YouWon()
    {
        if (youWon) return;
        youWon = true;

        OnGameEnded?.Invoke(true, "YOU SURVIVED, FOR NOW...");
    }

    private void ResetGameState()
    {
        // Se llama cuando se carga una nueva escena
        gameOver = false;
        youWon = false;
    }
}
