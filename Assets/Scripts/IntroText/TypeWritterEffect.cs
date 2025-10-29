using System.Collections;
using UnityEngine.Localization;
using UnityEngine;
using TMPro;

public class TypeWritterEffect : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textComponent;
    [SerializeField,TextArea] string fullText;
    [SerializeField] string dialogueKey; // Claves de localización
    [SerializeField] string tableName = "Story"; // Nombre de la String Table
    [SerializeField] float typingSpeed = 0.03f;
    [SerializeField] UIMainMenu ContinueBtn;

    void Start()
    {
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        fullText = string.Empty;
        var localizedLine = new LocalizedString(tableName, dialogueKey);
        var handle = localizedLine.GetLocalizedStringAsync();
        yield return handle;

        fullText = handle.Result;

        textComponent.text = "";
        foreach (char c in fullText)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void ContinueButton()
    {
        if (textComponent.text == fullText)
        {
            ContinueBtn.NextLevel();
        }
        else
        {
            StopAllCoroutines();
            textComponent.text = fullText;
        }
    }
}
