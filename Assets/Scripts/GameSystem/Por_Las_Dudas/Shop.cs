using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] TMP_Text moneyText;

    private void Update()
    {
        moneyText.text = "MONEY: " + GameManager.Instance.GetMoney();
    }

    public void BuyItem(int price, string itemId)
    {
        if (GameManager.Instance.SpendMoney(price))
        {
            PlayerInventory.Instance.BuyItem(ParseItemId(itemId));
            Debug.Log("Compraste: " + itemId);
        }
        else
        {
            Debug.Log("Dinero insuficiente");
        }
    }

    private ItemID ParseItemId(string id)
    {
        if (System.Enum.TryParse(id, out ItemID result))
        {
            return result;
        }
        else
        {
            Debug.LogError($"ItemID inválido: {id}");
            return default;
        }
    }
}
