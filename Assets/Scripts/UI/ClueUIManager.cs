using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ClueUIManager : MonoBehaviour
{
    [Header("Notebook UI")]
    [SerializeField] private GameObject notebookUI; // Panel de la libreta
    [SerializeField] private RectTransform clueListParent; // Donde instanciamos los slots
    [SerializeField] private GameObject clueSlotPrefab; // Prefab del slot de pista

    private List<TextMeshProUGUI> clueTexts = new List<TextMeshProUGUI>();

    private void Start()
    {
        CloseNotebook();
    }

    public void SetupNotebook(int requiredClues)
    {
        foreach (Transform child in clueListParent)
        {
            Destroy(child.gameObject);
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
            notebookUI.SetActive(!notebookUI.activeSelf);
    }

    public void CloseNotebook()
    {
        if (notebookUI != null)
            notebookUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleNotebook();
        }
    }
}
