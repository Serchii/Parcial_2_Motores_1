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

    [SerializeField] bool isTrigger = false;
    [SerializeField] bool dialogueTriggered = false;

    [SerializeField] bool isPlayerInRange;
    [SerializeField] bool didDialogueStart;
    [SerializeField] int lineIndex;
    [SerializeField] float typingTime = 0.05f;

    [Header("Sonido")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip dialogVoice;
    [SerializeField] float pitchVoice = 1f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Interact") && isPlayerInRange && !dialogueTriggered)
        {
            HandleInputOrSkip();
        }
    }

    void HandleInputOrSkip()
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
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        didDialogueStart = false;
        dialoguePanel.SetActive(false);
        //dialogueMark.SetActive(true);
        Time.timeScale = 1;
        if(isTrigger)
            dialogueTriggered = true; 
    }

    IEnumerator ShowLine()
    {
        int index = 0;
        dialogueText.text = string.Empty;

        var localizedLine = new LocalizedString(tableName, dialogueKeys[lineIndex]);
        var handle = localizedLine.GetLocalizedStringAsync();
        yield return handle;

        currentDialog = handle.Result;

        bool insideTag = false;
        string tagBuffer = "";

        foreach (char ch in currentDialog)
        {
            if (ch == '<')
            {
                insideTag = true;
                tagBuffer += ch;
                continue;
            }
            else if (ch == '>' && insideTag)
            {
                tagBuffer += ch;
                dialogueText.text += tagBuffer; // Agregamos etiqueta completa de una
                tagBuffer = "";
                insideTag = false;
                continue;
            }

            if (insideTag)
            {
                tagBuffer += ch;
                continue;
            }

            dialogueText.text += ch;
            yield return new WaitForSecondsRealtime(typingTime);

            if (audioSource != null && dialogVoice != null && index % 2 == 0)
            {
                audioSource.clip = dialogVoice;
                audioSource.pitch = Random.Range(pitchVoice - 0.05f, pitchVoice + 0.05f);
                audioSource.Play();
            }

            index++;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        
        isPlayerInRange = true;

        if (isTrigger && !dialogueTriggered)
        {
            StartDialogue();
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
