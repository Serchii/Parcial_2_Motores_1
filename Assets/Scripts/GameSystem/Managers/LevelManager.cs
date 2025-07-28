using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private ClueUIManager clueUIManager;
    [SerializeField] private int requiredClues = 2;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clueFound;
    [SerializeField] private PuzzleTrigger puzzleTrigger;

    private List<string> collectedClues = new List<string>();

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
        collectedClues.Clear();
        clueUIManager?.SetupNotebook(requiredClues);
        audioSource = GetComponent<AudioSource>();

        if (puzzleTrigger != null)
        {
            puzzleTrigger.SetInteractuable(false);
        }
    }

    public void CollectClue(string clueName)
    {
        if (collectedClues.Count < requiredClues)
        {
            collectedClues.Add(clueName);
            clueUIManager?.RevealClue(collectedClues.Count - 1, clueName);

            Debug.Log($"Pistas recogidas: {collectedClues.Count}/{requiredClues}");

            if (audioSource != null && clueFound != null)
            {
                audioSource.clip = clueFound;
                audioSource.Play();

                if (MusicManager.Instance != null)
                {
                    MusicManager.Instance.FadeOutAndIn();
                }
            }

            if (collectedClues.Count >= requiredClues)
            {
                ActivatePuzzleIfExists();
            }
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
        collectedClues.Clear();
        clueUIManager?.SetupNotebook(requiredClues);
        Debug.Log("Nivel iniciado manualmente.");
    }
}
