using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class LevelManagerTemp : MonoBehaviour
{
    public static LevelManagerTemp Instance { get; private set; }

    public enum LevelType { Day, Night }
    public enum ObjectiveType { Door, Puzzle }

    [Header("Configuración del Nivel")]
    [SerializeField] private LevelType levelType = LevelType.Day;
    [SerializeField] private int requiredClues = 3;
    [SerializeField] private ObjectiveType objectiveType = ObjectiveType.Door;

    [Header("Referencias")]
    [SerializeField] private GameObject doorPrefab;    
    [SerializeField] private GameObject puzzlePrefab; 
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private ClueUIManager clueUIManager;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clueFound;

    [Header("UI Derrota")]
    [SerializeField] private GameObject defeatPanelDay;
    [SerializeField] private TextMeshProUGUI defeatMessageDay;
    [SerializeField] private GameObject defeatPanelNight;
    [SerializeField] private TextMeshProUGUI defeatMessageNight;

    private List<string> collectedClues = new List<string>();
    private bool objectiveSpawned = false;
    private bool defeatTriggered = false;

    private int startHour;
    private int startMinute;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Debug.Log("LevelManagerTemp inicializado y Singleton asignado.");
    }

    private void Start()
    {
        clueUIManager?.SetupNotebook(requiredClues);

        GameClock clock = GameClock.Instance;
        if (clock != null)
        {
            startHour = clock.GetHour();
            startMinute = clock.GetMinute();
        }

        defeatPanelDay?.SetActive(false);
        defeatPanelNight?.SetActive(false);

    }

    public void CollectClue(string clueName)
    {
        if (string.IsNullOrEmpty(clueName) || collectedClues.Contains(clueName)) return;

        collectedClues.Add(clueName);
        clueUIManager?.RevealClue(collectedClues.Count - 1, clueName);
        audioSource?.PlayOneShot(clueFound);

        if (!objectiveSpawned && collectedClues.Count >= requiredClues)
        {
            ActivateObjective();
        }
    }

    private void ActivateObjective()
    {
        objectiveSpawned = true;

        if (objectiveType == ObjectiveType.Puzzle)
        {
            if (puzzlePrefab != null)
            {
                if (!puzzlePrefab.activeInHierarchy)
                    puzzlePrefab.SetActive(true);
            }
        }
        else
        {
            if (doorPrefab != null)
            {
                if (!doorPrefab.activeInHierarchy)
                    doorPrefab.SetActive(true);
            }
        }
    }

    // Límite de tiempo - Comprobación desactivada temporalmente
    /*
    private void CheckTimeLimit()
    {
        if (defeatTriggered) return;

        GameClock clock = GameClock.Instance;
        if (clock == null) return;

        int hour = clock.GetHour();
        int minute = clock.GetMinute();

        // Condiciones para perder según el tipo de nivel y la hora actual
        if (levelType == LevelType.Day && (hour >= 12 || hour < 6))
            TriggerDefeat("El tiempo se terminó. Debes salir de la casa.");
        else if (levelType == LevelType.Night && (hour >= 4 && hour < 20))
            TriggerDefeat("Te desmayaste del cansancio.");
    }
    */

    private void TriggerDefeat(string message)
    {
        defeatTriggered = true;
        Time.timeScale = 0f;

        if (levelType == LevelType.Day)
        {
            defeatPanelDay?.SetActive(true);
            if (defeatMessageDay != null) defeatMessageDay.text = message;
        }
        else if (levelType == LevelType.Night)
        {
            defeatPanelNight?.SetActive(true);
            if (defeatMessageNight != null) defeatMessageNight.text = message;
        }

        Debug.Log("Nivel perdido: " + message);
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f;

        GameClock clock = GameClock.Instance;
        if (clock != null)
            clock.SetTime(startHour, startMinute);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
