using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private bool gameOver = false;
    private bool youWon = false;

    public PlayerData playerData = new PlayerData();

    public bool IsGameOver() => gameOver || youWon;

    public static event Action<bool, string> OnGameEnded;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            GameSceneManager.OnSceneFullyLoaded += ResetGameState;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        AddMoney(1000);
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

    public void AddMoney(int amount)
    {
        playerData.Money += amount;
        // Acá podrías disparar un evento para actualizar la UI
    }

    public bool SpendMoney(int amount)
    {
        if (playerData.Money >= amount)
        {
            playerData.Money -= amount;
            // También podrías disparar un evento acá
            return true;
        }
        return false;
    }

    public int GetMoney()
    {
        return playerData.Money;
    }
}
