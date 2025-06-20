using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public float SavedHealth { get; private set; } = 100f;
    public float SavedMaxHealth { get; private set; } = 100f;

    public int Money { get; private set; } = 0;

    private bool gameOver = false;
    private bool youWon = false;

    public static event Action<bool, string> OnGameEnded;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadMoney();
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

    private void ResetGameState()
    {
        gameOver = false;
        youWon = false;
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

    public void AddMoney(int amount)
    {
        Money += amount;
        PlayerPrefs.SetInt("Money", Money);
    }

    public bool SpendMoney(int amount)
    {
        if (Money >= amount)
        {
            Money -= amount;
            PlayerPrefs.SetInt("Money", Money);
            return true;
        }
        return false;
    }

    public void LoadMoney()
    {
        Money = PlayerPrefs.GetInt("Money", 0);
    }

    public bool IsGameOver()
    {
        return gameOver || youWon;
    }

    public void ResetGameData()
    {
        Money = 0;
        PlayerPrefs.SetInt("Money", Money);

        if (PlayerInventory.Instance != null)
            PlayerInventory.Instance.ResetInventory();

        PlayerPrefs.Save();
    }
    public int GetMoney()
    {
        return Money;
    }

    public void SavePlayerHealth(float health, float maxHealth)
    {
        SavedHealth = health;
        SavedMaxHealth = maxHealth;
    }

    public void ResetSavedHealth()
    {
        SavedHealth = 100f;
        SavedMaxHealth = 100f;
    }

    public void SetMaxHealth()
    {
        SavedHealth = SavedMaxHealth;
    }
}