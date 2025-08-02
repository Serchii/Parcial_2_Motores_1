using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Localization;
using System.Collections;
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

    [SerializeField] private string playerLayerName = "Player";
    [SerializeField] private string enemyLayerName = "Enemy";
    [SerializeField] string winKey;
    [SerializeField] string loseKey;
    [SerializeField] string tableName = "UI";
    [SerializeField] string currentTitle;

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

        ConfigureCollisions();
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
        StartCoroutine(GetTranslateText(loseKey,false));
    }

    public void YouWon()
    {
        if (youWon) return;
        StartCoroutine(GetTranslateText(winKey,true));
    }

    IEnumerator GetTranslateText(string dialogueKey, bool won)
    {
        currentTitle = string.Empty;
        var localizedLine = new LocalizedString(tableName, dialogueKey);
        var handle = localizedLine.GetLocalizedStringAsync();
        yield return handle;

        currentTitle = handle.Result;
        if (won)
        {
            youWon = true;
        }
        else
            gameOver = true;

        OnGameEnded?.Invoke(won, currentTitle);
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

    void ConfigureCollisions()
    {
        int playerLayer = LayerMask.NameToLayer(playerLayerName);
        int enemyLayer = LayerMask.NameToLayer(enemyLayerName);

        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer);
        Physics2D.IgnoreLayerCollision(enemyLayer, enemyLayer);
    }
}