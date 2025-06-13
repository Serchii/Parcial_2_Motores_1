using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] TMP_Text moneyText;

    private void Update()
    {
        moneyText.text = "MONEY: " + GameManager.Instance.GetMoney();
    }

    public void BuyItem(int price, string itemName)
    {
        if (GameManager.Instance.SpendMoney(price))
        {
            GameManager.Instance.playerData.Inventory.Add(itemName);
            Debug.Log("Compraste: " + itemName);
        }
        else
        {
            Debug.Log("Dinero insuficiente");
        }
    }
}