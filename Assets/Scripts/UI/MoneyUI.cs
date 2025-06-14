using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] private TMP_Text moneyText;

    void Update()
    {
        moneyText.text = "Money: " + GameManager.Instance.GetMoney();
    }
}

