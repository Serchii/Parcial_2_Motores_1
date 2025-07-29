using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private string moneyTranslated;
    [SerializeField] private string moneyTextKey;
    [SerializeField] string tableName = "UI"; // Nombre de la String Table

    void Start()
    {
        var localizedLine = new LocalizedString(tableName, moneyTextKey);
        localizedLine.StringChanged += OnTranslationChanged; // Se llama cuando la traducción esté lista y también si cambia de idioma
    }

    private void OnTranslationChanged(string translated)
    {
        moneyTranslated = translated;
    }

    void Update()
    {
        moneyText.text = moneyTranslated + GameManager.Instance.GetMoney();
    }
}

