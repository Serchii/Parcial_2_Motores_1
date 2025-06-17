using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int StartingMoney = 1000;
    private int money;

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
            GameSceneManager.OnSceneFullyLoaded -= ResetGameState;
    }

    public bool IsGameOver() => gameOver || youWon;

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

    public void AddMoney(int amount)
    {
        money += amount;
        SaveData();
    }

    public bool SpendMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            SaveData();
            return true;
        }
        return false;
    }

    public int GetMoney() => money;

    private void SaveData()
    {
        PlayerPrefs.SetInt("Money", money);
        PlayerInventory.Instance.SaveData();
    }

    private void LoadData()
    {
        money = PlayerPrefs.GetInt("Money", StartingMoney);
        PlayerInventory.Instance.LoadData();
    }
}
