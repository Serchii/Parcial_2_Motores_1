using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Configuración del nivel")]
    public LevelData currentLevelData;

    [Header("Horario diurno")]
    public int dayStartHour = 6;
    public int dayEndHour = 12;

    [Header("Horario nocturno")]
    public int nightStartHour = 20;
    public int nightEndHour = 4;

    private int currentSceneIndex = 0;
    private int cluesCollected = 0;
    private bool levelRunning = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (currentLevelData == null)
        {
            Debug.LogError("No hay LevelData asignado en LevelManager.");
            return;
        }

    }

    private void Update()
    {
        if (!levelRunning || GameClock.Instance == null) return;

        int h = GameClock.Instance.hour;
        if (!IsWithinAllowedTime(h))
        {
            Debug.LogWarning("¡Fuera de horario! Nivel REINICIADO.");
            OnPlayerLose();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(WaitForGameClockThenInit(scene));
    }

    private IEnumerator WaitForGameClockThenInit(Scene scene)
    {
        while (GameClock.Instance == null)
            yield return null;

        yield return new WaitForSeconds(0.5f);

        if (currentSceneIndex < currentLevelData.levelScenes.Length &&
            scene.name == currentLevelData.levelScenes[currentSceneIndex])
        {
            int h = GameClock.Instance.hour;
            if (!IsWithinAllowedTime(h))
            {
                Debug.LogWarning("Hora inválida al cargar escena. Forzando reinicio.");
                OnPlayerLose();
                yield break;
            }

            cluesCollected = 0;

            while (ClueUIManager.Instance == null)
                yield return null;

            Debug.Log("SetClueGoal desde LevelManager...");
            ClueUIManager.Instance.SetClueGoal(currentLevelData.requiredClues);
        }
    }


    private bool IsWithinAllowedTime(int hour)
    {
        if (currentLevelData.levelType == LevelType.Day)
            return hour >= dayStartHour && hour < dayEndHour;
        else
            return hour >= nightStartHour || hour < nightEndHour;
    }

    public void StartLevelManualmente()
    {
        if (!levelRunning)
        {
            levelRunning = true;
            Debug.Log("Nivel iniciado manualmente.");
        }
    }

    public void CollectClue()
    {
        if (!levelRunning) return;

        cluesCollected++;
        Debug.Log($"Pistas recogidas: {cluesCollected}/{currentLevelData.requiredClues}");

        ClueUIManager.Instance?.AddClue();

        if (cluesCollected >= currentLevelData.requiredClues)
        {
            Debug.Log("Objetivo de pistas completado. Avanzando a la siguiente escena...");
            NextScene();
        }
    }

    public void OnPlayerLose()
    {
        levelRunning = false;
        ClockStateManager.Instance?.RequestRestore();
        LoadSceneAtIndex(currentSceneIndex, false);
    }

    private void NextScene()
    {
        levelRunning = false;

        if (currentSceneIndex + 1 < currentLevelData.levelScenes.Length)
        {
            ClockStateManager.Instance?.SaveClockTime();
            LoadSceneAtIndex(currentSceneIndex + 1, false);
        }
        else
        {
            OnLevelWin();
        }
    }

    private void OnLevelWin()
    {
        levelRunning = false;
        Debug.Log("¡Nivel completado con éxito!");
    }

    private void LoadSceneAtIndex(int index, bool saveTime)
    {
        currentSceneIndex = index;
        if (saveTime)
            ClockStateManager.Instance?.SaveClockTime();

        SceneManager.LoadScene(currentLevelData.levelScenes[index]);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void SetLevelTime(int h, int m)
    {
        if (GameClock.Instance != null)
        {
            GameClock.Instance.SetTime(h, m);
            Debug.Log($"Hora del nivel modificada a {h:00}:{m:00}");
        }
        else
        {
            Debug.LogWarning("GameClock no está activo, no se puede cambiar la hora.");
        }
    }
}
