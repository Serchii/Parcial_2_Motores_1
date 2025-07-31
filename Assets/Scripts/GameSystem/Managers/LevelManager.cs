using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private ClueUIManager clueUIManager;
    [SerializeField] private PuzzleTrigger puzzleTrigger;
    [SerializeField] private DetectObjective detectObjective;
    [SerializeField] private GameObject sceneTransitionDoor;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clueFound;

    private LevelData currentLevel;
    private List<string> collectedClues = new List<string>();
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
        enabled = false;  // Desactivado por defecto hasta cargar escena válida
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (GameSession.Instance == null || GameSession.Instance.currentLevelData == null)
        {
            Debug.LogWarning("No hay nivel activo en GameSession. LevelManager desactivado.");
            enabled = false;
            return;
        }

        currentLevel = GameSession.Instance.currentLevelData;

        bool perteneceAlNivel = false;
        int indexEscena = -1;

        for (int i = 0; i < currentLevel.levelScenes.Length; i++)
        {
            if (currentLevel.levelScenes[i] == scene.name)
            {
                perteneceAlNivel = true;
                indexEscena = i;
                break;
            }
        }

        if (!perteneceAlNivel)
        {
            Debug.Log($"Escena '{scene.name}' no pertenece al nivel '{currentLevel.name}'. LevelManager desactivado.");
            enabled = false;
            return;
        }

        enabled = true;
        Debug.Log($"LevelManager activado para escena '{scene.name}' del nivel '{currentLevel.name}'.");

        if (indexEscena == 0)
        {
            StartLevelFromSession();
        }

        if (sceneTransitionDoor == null)
        {
            var puerta = GameObject.FindWithTag("SceneDoor");
            if (puerta != null)
            {
                sceneTransitionDoor = puerta;
                sceneTransitionDoor.SetActive(false);
            }
        }

        if (puzzleTrigger == null)
        {
            puzzleTrigger = FindObjectOfType<PuzzleTrigger>();
            if (puzzleTrigger != null)
                puzzleTrigger.SetInteractuable(false);
        }
    }

    public void StartLevelFromSession()
    {
        currentLevel = GameSession.Instance.currentLevelData;
        if (currentLevel == null)
        {
            Debug.LogWarning("GameSession no tiene nivel activo. LevelManager desactivado.");
            enabled = false;
            return;
        }

        collectedClues.Clear();
        registeredDoors.Clear();

        clueUIManager?.SetupNotebook(currentLevel.requiredClues);

        Debug.Log($"Nivel '{currentLevel.name}' iniciado en LevelManager.");
    }

    public void CollectClue(string clueName)
    {
        if (currentLevel == null)
        {
            Debug.LogWarning("No hay nivel activo. No se puede recolectar la pista.");
            return;
        }
        if (string.IsNullOrEmpty(clueName)) return;
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
        if (currentLevel == null) return;

        if (currentLevel.objectiveType == ObjectiveType.Puzzle)
        {
            if (puzzleTrigger != null)
                puzzleTrigger.SetInteractuable(true);
        }
        else
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
        }
    }

    public void RegisterSceneDoor(GameObject door)
    {
        if (!registeredDoors.Contains(door))
        {
            registeredDoors.Add(door);
            door.SetActive(false);
        }
    }
}
