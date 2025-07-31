using UnityEngine;
using System.Collections;
using UnityEngine.Localization;
using TMPro;

public class InteractableShopTrigger : MonoBehaviour
{
    [Header("Interaction Prompt")]
    [SerializeField] GameObject interactionPromptPrefab;
    [SerializeField] string textToShowKey = "InteractText";
    [SerializeField] string tableName = "UI";
    [SerializeField] string textPrompt;
    private GameObject promptInstance;
    private bool _playerInRange = false;

    void Update()
    {
        if (_playerInRange && Input.GetButtonDown("Interact"))
        {
            ShopUI.Instance.OpenShop();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = true;

            ShowPrompt();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = false;
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

        UIFollowPlayer followScript = promptInstance.GetComponent<UIFollowPlayer>();
        followScript?.SetTarget(this.transform);
        
    }
}