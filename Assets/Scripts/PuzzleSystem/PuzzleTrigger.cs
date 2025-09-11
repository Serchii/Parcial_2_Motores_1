using UnityEngine;
using System.Collections;
using UnityEngine.Localization;
using TMPro;
using Unity.VisualScripting;

public class PuzzleTrigger : MonoBehaviour
{
    [SerializeField] GameObject uiPuzzle;
    [SerializeField] GameObject interactionPromptPrefab;
    [SerializeField] string textToShowKey;
    [SerializeField] string tableName = "UI";
    [SerializeField] string textPrompt;
    [SerializeField] bool isInteractuable = false;
    [SerializeField] private PuzzleGridManager puzzleManager;
    [SerializeField] bool followPlayer = true;
    
    [SerializeField] GameObject[] objectsToActivate;

    private GameObject promptInstance;
    private bool canActivate = false;
    private Transform player;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerAttack playerAttack;

    void Start()
    {
        if (puzzleManager != null)
        {
            puzzleManager.OnCompleted += PuzzleCompleted;
        }

        if (textToShowKey.Length == 0)
        {
            textToShowKey = "InteractText";
        }
    }

    void Update()
    {
        if (canActivate && Input.GetButtonDown("Interact") && !uiPuzzle.activeSelf && GameStateManager.Instance.StateMachine.CurrentState == GameStateManager.Instance.Gameplay)
        {
            if (isInteractuable)
            {
                HidePrompt();
                ActivateObjects();
                uiPuzzle.SetActive(true);
                //SetPlayerActive(false);
                GameStateManager.Instance.EnterPuzzle();
            }
        }

        if (uiPuzzle != null && uiPuzzle.activeSelf && Input.GetButtonDown("Cancel"))
        {
            if (isInteractuable)
            {
                uiPuzzle.SetActive(false);
                //SetPlayerActive(true);
                GameStateManager.Instance.ExitPuzzle();
            }

            if (canActivate) ShowPrompt();
        }
    }

    IEnumerator GetTranslateText(string dialogueKey)
    {
        textPrompt = string.Empty;
        var localizedLine = new LocalizedString(tableName, dialogueKey);
        var handle = localizedLine.GetLocalizedStringAsync();
        yield return handle;

        textPrompt = handle.Result;

        var textComponent = promptInstance.GetComponentInChildren<TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = textPrompt;
        }

        if (followPlayer)
        {
            UIFollowPlayer followScript = promptInstance.GetComponent<UIFollowPlayer>();
            followScript?.SetTarget(player);
        }
        else
        {
            UIFollowPlayer followScript = promptInstance.GetComponent<UIFollowPlayer>();
            followScript?.SetTarget(this.transform);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        canActivate = true;
        player = other.transform;

        playerMovement = player.GetComponent<PlayerMovement>();
        playerAttack = player.GetComponent<PlayerAttack>();

        if (!uiPuzzle.activeSelf)
        {
            ShowPrompt();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canActivate = false;
            HidePrompt();
        }
    }

    private void ShowPrompt()
    {
        if (interactionPromptPrefab != null && promptInstance == null)
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            promptInstance = Instantiate(interactionPromptPrefab, canvas.transform);

            StartCoroutine(GetTranslateText(textToShowKey));
        }
    }

    private void HidePrompt()
    {
        if (promptInstance != null)
        {
            Destroy(promptInstance);
            promptInstance = null;
        }
    }

    void OnDestroy()
    {
        if (puzzleManager != null)
            puzzleManager.OnCompleted -= PuzzleCompleted;
    }

    void PuzzleCompleted()
    {
        Invoke("EndPuzzle", 1f);
    }

    void EndPuzzle()
    {
        uiPuzzle.SetActive(false);
        GameStateManager.Instance.ExitPuzzle();
        HidePrompt();
        Destroy(gameObject);
    }

    public void SetInteractuable(bool value)
    {
        isInteractuable = value;
        if (!value)
            HidePrompt();
    }

    public void ActivateObjects()
    {
        if (objectsToActivate.Length > 0)
        {
            foreach (GameObject obj in objectsToActivate)
            {
                obj.SetActive(true);
            }
        }
    }
}