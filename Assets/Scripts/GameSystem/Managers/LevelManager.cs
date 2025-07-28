using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Configuración de niveles")]
    [SerializeField] private LevelCollection levelCollection;
    private int currentLevelIndex = 0;
    private LevelData currentLevel;

    [Header("UI y pistas")]
    [SerializeField] private ClueUIManager clueUIManager;
    private List<string> collectedClues = new List<string>();

    [Header("Objetivo")]
    [SerializeField] private PuzzleTrigger puzzleTrigger;
    [SerializeField] private DetectObjective detectObjective;
    [SerializeField] private GameObject sceneTransitionDoor;

    [Header("Sonido")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clueFound;

    private List<GameObject> registeredDoors = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        StartLevel(0);
    }

    public void StartLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levelCollection.levels.Length)
        {
            Debug.LogError("Índice de nivel fuera de rango");
            return;
        }

        currentLevelIndex = levelIndex;
        currentLevel = levelCollection.levels[levelIndex];
        collectedClues.Clear();
        registeredDoors.Clear();

        clueUIManager?.SetupNotebook(currentLevel.requiredClues);
        LoadLevelScenes(currentLevel.levelScenes);

        Debug.Log($"Iniciando nivel {levelIndex}: {currentLevel.name}");
    }

    public void StartLevelManualmente()
    {
        StartLevel(currentLevelIndex);
    }

    private void LoadLevelScenes(string[] scenes)
    {
        if (scenes.Length == 0) return;

        foreach (string sceneName in scenes)
        {
            if (IsSceneInBuild(sceneName))
            {
                SceneManager.LoadScene(sceneName, sceneName == scenes[0] ? LoadSceneMode.Single : LoadSceneMode.Additive);
            }
            else
            {
                Debug.LogError($"La escena '{sceneName}' no está en Build Settings y no se puede cargar.");
            }
        }
    }

    private bool IsSceneInBuild(string sceneName)
    {
        int count = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < count; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            if (name == sceneName) return true;
        }
        return false;
    }

    public void RegisterSceneDoor(GameObject door)
    {
        if (!registeredDoors.Contains(door))
        {
            registeredDoors.Add(door);
            door.SetActive(false);
        }
    }

    public void CollectClue(string clueName)
    {
        if (collectedClues.Contains(clueName)) return;

        collectedClues.Add(clueName);
        clueUIManager?.RevealClue(collectedClues.Count - 1, clueName);

        if (audioSource != null && clueFound != null)
        {
            audioSource.clip = clueFound;
            audioSource.Play();
        }

        if (collectedClues.Count >= currentLevel.requiredClues)
        {
            ActivateObjective();
        }
    }

    private void ActivateObjective()
    {
        if (currentLevel == null)
        {
            Debug.LogWarning("No hay nivel activo.");
            return;
        }

        if (currentLevel.objectiveType == ObjectiveType.Puzzle)
        {
            if (puzzleTrigger != null)
            {
                puzzleTrigger.SetInteractuable(true);
                Debug.Log("Puzzle activado.");
            }
        }
        else if (currentLevel.objectiveType == ObjectiveType.Door)
        {
            foreach (var door in registeredDoors)
            {
                if (door != null)
                    door.SetActive(true);
            }

            if (sceneTransitionDoor != null)
                sceneTransitionDoor.SetActive(true);
            else if (detectObjective != null)
                detectObjective.ActivateDoorDirectly();
            else
                Debug.LogWarning("No se encontró puerta para activar.");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (sceneTransitionDoor == null)
        {
            GameObject foundDoor = GameObject.FindWithTag("SceneDoor");
            if (foundDoor != null)
            {
                sceneTransitionDoor = foundDoor;
                sceneTransitionDoor.SetActive(false);
                Debug.Log("Puerta con tag asignada automáticamente.");
            }
        }

        if (puzzleTrigger == null)
        {
            puzzleTrigger = FindObjectOfType<PuzzleTrigger>();
            if (puzzleTrigger != null)
                puzzleTrigger.SetInteractuable(false);
        }
    }

    public void LoadNextLevel()
    {
        int nextIndex = currentLevelIndex + 1;
        if (nextIndex < levelCollection.levels.Length)
        {
            StartLevel(nextIndex);
        }
        else
        {
            Debug.Log("No hay más niveles.");
        }
    }

    public int GetRequiredClues() => currentLevel != null ? currentLevel.requiredClues : 0;
}
