using TMPro;
using UnityEngine;

public class ClueUIManager : MonoBehaviour
{
    public static ClueUIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI clueText;

    private int max = 0;
    private int current = 0;

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
        if (LevelManager.Instance != null && LevelManager.Instance.currentLevelData != null)
        {
            SetClueGoal(LevelManager.Instance.currentLevelData.requiredClues);
        }
    }

    public void SetClueGoal(int maxClues)
    {
        max = maxClues;
        current = 0;
        Debug.Log($"[ClueUIManager] SetClueGoal llamado: {current}/{max}");
        UpdateUI();
    }

    public void AddClue()
    {
        current++;
        Debug.Log($"[ClueUIManager] AddClue: {current}/{max}");
        UpdateUI();
    }

    private void UpdateUI()
    {
        clueText.text = $"{current} / {max}";
    }
}
