using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private ClueUIManager clueUIManager;
    [SerializeField] private int requiredClues = 2;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip clueFound;
    private int currentClues = 0;

    [SerializeField] private PuzzleTrigger puzzleTrigger;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        currentClues = 0;
        clueUIManager?.UpdateClueUI(currentClues, requiredClues);
        audioSource = GetComponent<AudioSource>();

        if (puzzleTrigger != null)
        {
            puzzleTrigger.SetInteractuable(false);
        }
    }

    public void CollectClue()
    {
        currentClues++;
        clueUIManager?.UpdateClueUI(currentClues, requiredClues);
        Debug.Log($"Pistas recogidas: {currentClues}/{requiredClues}");

        if (audioSource != null && clueFound != null)
        {
            audioSource.clip = clueFound;
            audioSource.Play();

            if (MusicManager.Instance != null)
            {
                MusicManager.Instance.FadeOutAndIn();
            }
        }

        if (currentClues >= requiredClues)
            {
                ActivatePuzzleIfExists();
            }
    }

    private void ActivatePuzzleIfExists()
    {
        if (puzzleTrigger != null)
        {
            puzzleTrigger.SetInteractuable(true);
            Debug.Log("Puzzle activado desde LevelManager.");
        }
        else
        {
            Debug.LogWarning("No hay PuzzleTrigger asignado al LevelManager.");
        }
    }

    public void SetRequiredClues(int value)
    {
        requiredClues = value;
    }

    public void StartLevelManualmente()
    {
        currentClues = 0;
        clueUIManager?.UpdateClueUI(currentClues, requiredClues);
        Debug.Log("Nivel iniciado manualmente.");
    }
}