using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public PlayerData playerData = new PlayerData();

    private bool gameOver = false;
    private bool youWon = false;

    public static event Action<bool, string> OnGameEnded;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData();
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
    public bool IsGameOver()
    {
        return gameOver || youWon;
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
        gameOver = false;
        youWon = false;
    }

    // 🪙 Dinero
    public void AddMoney(int amount)
    {
        playerData.Money += amount;
        SaveData();
    }

    public bool SpendMoney(int amount)
    {
        if (playerData.Money >= amount)
        {
            playerData.Money -= amount;
            SaveData();
            return true;
        }
        return false;
    }

    public int GetMoney() => playerData.Money;

    // 🎒 Inventario
    public bool HasItem(string itemId) => playerData.Inventory.Contains(itemId);

    public void AddItem(string itemId)
    {
        playerData.Inventory.Add(itemId);
        SaveData();
    }

    // 💾 Persistencia
    private void SaveData()
    {
        PlayerPrefs.SetInt("Money", playerData.Money);
        PlayerPrefs.SetString("Inventory", string.Join(",", playerData.Inventory));
    }

    private void LoadData()
    {
        playerData.Money = PlayerPrefs.GetInt("Money", 1000);

        string savedInventory = PlayerPrefs.GetString("Inventory", "");
        playerData.Inventory = new HashSet<string>(savedInventory.Split(',', StringSplitOptions.RemoveEmptyEntries));
    }
}
