using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public enum LevelType { Day, Night }

    [Header("Configuración del nivel")]
    [SerializeField] private LevelType levelType = LevelType.Day;
    [Tooltip("Lista de escenas que componen este nivel, en orden")]
    [SerializeField] private string[] levelScenes;
    [Tooltip("Cantidad de pistas/clues necesarias para avanzar de escena")]
    [SerializeField] private int requiredClues = 3;

    [Header("Horario diurno")]
    [SerializeField] private int dayStartHour = 6;
    [SerializeField] private int dayEndHour = 12;

    [Header("Horario nocturno")]
    [SerializeField] private int nightStartHour = 20;
    [SerializeField] private int nightEndHour = 4;

    private int currentSceneIndex = 0;
    private int cluesCollected = 0;
    private bool levelRunning = false;

    private void Awake()
    {
        // Singleton simple para no duplicar el LevelManager
        if (FindObjectsOfType<LevelManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        // Inicia el nivel en la primera escena
        LoadSceneAtIndex(0, saveTime: true);
    }

    private void Update()
    {
        if (!levelRunning || GameClock.Instance == null) return;

        // Verifica cada frame si te pasaste del rango horario
        int h = GameClock.Instance.hour;
        if (!IsWithinAllowedTime(h))
        {
            Debug.LogWarning("¡Fuera de horario! Nivel REINICIADO.");
            OnPlayerLose();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Cuando cargamos la escena correcta, activamos el nivel
        if (currentSceneIndex < levelScenes.Length &&
            scene.name == levelScenes[currentSceneIndex])
        {
            levelRunning = true;
            cluesCollected = 0;
            Debug.Log($"Nivel: escena '{scene.name}' iniciada. Hora: {GameClock.Instance.hour:00}:{GameClock.Instance.minute:00}");
        }
    }

    private bool IsWithinAllowedTime(int hour)
    {
        if (levelType == LevelType.Day)
            return hour >= dayStartHour && hour < dayEndHour;
        else // Night
            // Puede abarcar trasnoche
            return hour >= nightStartHour || hour < nightEndHour;
    }

    /// <summary>
    /// Llamar desde tu lógica de pistas cada vez que recojas una.
    /// </summary>
    public void CollectClue()
    {
        if (!levelRunning) return;
        cluesCollected++;
        Debug.Log($"Pistas recogidas: {cluesCollected}/{requiredClues}");

        if (cluesCollected >= requiredClues)
        {
            Debug.Log("Objetivo de pistas completado. Avanzando a la siguiente escena...");
            NextScene();
        }
    }

    /// <summary>
    /// Llamar cuando el jugador muere o falla.
    /// </summary>
    public void OnPlayerLose()
    {
        levelRunning = false;

        // Pedimos restaurar la hora guardada
        ClockStateManager.Instance.RequestRestore();

        // Recargamos la misma escena
        LoadSceneAtIndex(currentSceneIndex, saveTime: false);
    }

    /// <summary>
    /// Avanza a la siguiente escena; si no hay más, gana el nivel.
    /// </summary>
    private void NextScene()
    {
        levelRunning = false;

        if (currentSceneIndex + 1 < levelScenes.Length)
        {
            // Guardamos el tiempo actual para la próxima escena
            ClockStateManager.Instance.SaveClockTime();
            LoadSceneAtIndex(currentSceneIndex + 1, saveTime: false);
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
        // Aquí podrías cargar un menú de victoria, avanzar al siguiente nivel, etc.
    }

    /// <summary>
    /// Carga la escena indicada y opcionalmente guarda el tiempo inicial de esa escena.
    /// </summary>
    private void LoadSceneAtIndex(int index, bool saveTime)
    {
        currentSceneIndex = index;
        if (saveTime)
            ClockStateManager.Instance.SaveClockTime();

        SceneManager.LoadScene(levelScenes[index]);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
