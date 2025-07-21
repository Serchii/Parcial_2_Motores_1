using UnityEngine;
using TMPro;

public class ClueUIManager : MonoBehaviour
{
    public static ClueUIManager Instance;

    public TextMeshProUGUI clueText;
    private int current = 0;
    private int max = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void SetClueGoal(int maxClues)
    {
        max = maxClues;
        current = 0;
        UpdateUI();
    }

    public void AddClue()
    {
        current++;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (clueText != null)
            clueText.text = $"Pistas: {current} / {max}";
    }
}
