using UnityEngine;
using TMPro;

public class ClueUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI clueText;

    public void UpdateClueUI(int current, int required)
    {
        if (clueText != null)
        {
            clueText.text = $"{current}/{required}";
        }
        else
        {
            Debug.LogWarning("No hay TextMeshProUGUI asignado a ClueUIManager.");
        }
    }
}
