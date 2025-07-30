using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ClueUIManager : MonoBehaviour
{
    [Header("Notebook UI")]
    [SerializeField] private GameObject notebookUI;
    [SerializeField] private RectTransform clueListParent;
    [SerializeField] private GameObject clueSlotPrefab;

    private List<TextMeshProUGUI> clueTexts = new List<TextMeshProUGUI>();

    private void Start()
    {
        CloseNotebook();
    }

    public void SetupNotebook(int requiredClues)
    {
        if (clueListParent == null)
        {
            Debug.LogWarning("clueListParent es null o destruido en SetupNotebook.");
            return;
        }

        for (int i = clueListParent.childCount - 1; i >= 0; i--)
        {
            Destroy(clueListParent.GetChild(i).gameObject);
        }

        clueTexts.Clear();

        for (int i = 0; i < requiredClues; i++)
        {
            GameObject slot = Instantiate(clueSlotPrefab, clueListParent);
            var text = slot.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = "?";
                clueTexts.Add(text);
            }
            else
            {
                Debug.LogWarning("No se encontró TextMeshProUGUI en el slot de pista.");
            }
        }
    }

    public void RevealClue(int index, string clueName)
    {
        if (index >= 0 && index < clueTexts.Count)
        {
            clueTexts[index].text = clueName;
        }
    }

    public void ToggleNotebook()
    {
        if (notebookUI != null)
        {
            bool isActive = !notebookUI.activeSelf;
            notebookUI.SetActive(isActive);
            Debug.Log("Notebook " + (isActive ? "abierto" : "cerrado"));
        }
    }

    public void CloseNotebook()
    {
        if (notebookUI != null)
        {
            notebookUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleNotebook();
        }
    }
}
