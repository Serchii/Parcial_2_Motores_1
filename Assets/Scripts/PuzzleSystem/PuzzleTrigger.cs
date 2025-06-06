using UnityEngine;
using TMPro;

public class PuzzleTrigger : MonoBehaviour
{
    [SerializeField] GameObject uiPuzzle;
    [SerializeField] GameObject interactionPromptPrefab;
    [SerializeField] string textPrompt = "[E] to Interact";

    private GameObject promptInstance;
    private bool canActivate = false;
    private Transform player;
    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;

    void Update()
    {
        if (canActivate && Input.GetButtonDown("Up") && !uiPuzzle.activeSelf)
        {
            HidePrompt();
            uiPuzzle.SetActive(true);
            SetPlayerActive(false);
        }

        if (uiPuzzle.activeSelf && Input.GetButtonDown("Cancel"))
        {
            uiPuzzle.SetActive(false);
            SetPlayerActive(true);

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
            playerMovement.enabled = isActive;

        if (playerAttack != null)
            playerAttack.enabled = isActive;
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
