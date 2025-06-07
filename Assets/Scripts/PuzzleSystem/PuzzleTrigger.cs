using UnityEngine;
using TMPro;

public class PuzzleTrigger : MonoBehaviour
{
    [SerializeField] GameObject uiPuzzle;
    [SerializeField] GameObject interactionPromptPrefab;
    [SerializeField] string textPrompt = "[W] to Interact";
    [SerializeField] bool isInteractuable = true;

    private GameObject promptInstance;
    private bool canActivate = false;
    private Transform player;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerAttack playerAttack;

    void Update()
    {
        if (canActivate && Input.GetButtonDown("Up") && !uiPuzzle.activeSelf)
        {
            if(isInteractuable)
            {
                HidePrompt();
                uiPuzzle.SetActive(true);
                SetPlayerActive(false);
            }
            
        }

        if (uiPuzzle.activeSelf && Input.GetButtonDown("Cancel"))
        {
            if(isInteractuable)
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

            // Cambiar el texto
            var textComponent = promptInstance.GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = textPrompt;
            }

            // Seguir al jugador
            UIFollowPlayer followScript = promptInstance.GetComponent<UIFollowPlayer>();
            followScript?.SetTarget(player);
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
}
