using UnityEngine;
using TMPro;

public class PuzzleTrigger : MonoBehaviour
{
    [SerializeField] GameObject uiPuzzle;
    [SerializeField] GameObject interactionPromptPrefab;
    [SerializeField] string textPrompt = "[E] to Interact";
    [SerializeField] bool isInteractuable = true;
    [SerializeField] private PuzzleGridManager puzzleManager;
    [SerializeField] bool followPlayer = true; 

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
    }
    void Update()
    {
        if (canActivate && Input.GetButtonDown("Interact") && !uiPuzzle.activeSelf)
        {
            if (isInteractuable)
            {
                HidePrompt();
                uiPuzzle.SetActive(true);
                SetPlayerActive(false);
            }

        }

        if (uiPuzzle.activeSelf && Input.GetButtonDown("Cancel"))
        {
            if (isInteractuable)
            {
                uiPuzzle.SetActive(false);
                SetPlayerActive(true);
            }

            if (canActivate) ShowPrompt();
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

    private void SetPlayerActive(bool isActive)
    {
        if (playerMovement != null)
        {
            playerMovement.enabled = isActive;
            Debug.Log($"Movimiento: {isActive}");
        }

        if (playerAttack != null)
        {
            playerAttack.enabled = isActive;
            Debug.Log($"Ataque: {isActive}");
        }


    }

    private void ShowPrompt()
    {
        if (interactionPromptPrefab != null && promptInstance == null)
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            promptInstance = Instantiate(interactionPromptPrefab, canvas.transform);

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
        {
            puzzleManager.OnCompleted -= PuzzleCompleted;
        }
    }

    void PuzzleCompleted()
    {
        Invoke("EndPuzzle", 1f);
    }

    void EndPuzzle()
    {
        uiPuzzle.SetActive(false);
        SetPlayerActive(true);
        HidePrompt();
        Destroy(gameObject);
    }
}
