using System.Collections;
using UnityEngine.Localization;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    [SerializeField] GameObject dialogueMark;
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] string[] dialogueKeys; // Claves de localización
    [SerializeField] string tableName = "Dialogues"; // Nombre de la String Table
    [SerializeField] string currentDialog;

    [SerializeField] bool isPlayerInRange;
    [SerializeField] bool didDialogueStart;
    [SerializeField] int lineIndex;
    [SerializeField] float typingTime = 0.05f;

    void Update()
    {
        if (isPlayerInRange && Input.GetButtonDown("Interact"))
        {
            if (!didDialogueStart)
            {
                StartDialogue();
            }
            else if (dialogueText.text == currentDialog)
            {
                NextDialogueLine();
            }
            else
            {
                StopAllCoroutines();
                dialogueText.text = currentDialog;
            }
        }
    }

    void StartDialogue()
    {
        didDialogueStart = true;
        dialoguePanel.SetActive(true);
        //dialogueMark.SetActive(false);
        lineIndex = 0;
        Time.timeScale = 0;
        StartCoroutine(ShowLine());
    }

    void NextDialogueLine()
    {
        lineIndex++;

        if (lineIndex < dialogueKeys.Length)
        {
            StartCoroutine(ShowLine());
        }
        else
        {
            didDialogueStart = false;
            dialoguePanel.SetActive(false);
            //dialogueMark.SetActive(true);
            Time.timeScale = 1;
        }
    }

    IEnumerator ShowLine()
    {
        dialogueText.text = string.Empty;

        // Obtener texto traducido desde Localization
        var localizedLine = new LocalizedString(tableName, dialogueKeys[lineIndex]);
        var handle = localizedLine.GetLocalizedStringAsync();
        yield return handle;

        currentDialog = handle.Result;

        foreach (char ch in currentDialog)
        {
            dialogueText.text += ch;
            yield return new WaitForSecondsRealtime(typingTime);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            //dialogueMark.SetActive(true);
            Debug.Log("Se puede dialogar");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            //dialogueMark.SetActive(false);
            Debug.Log("No mas dialogo");
        }
    }
}
