using TMPro;
using UnityEngine;

public class ShopItemUI : MonoBehaviour
{
    [SerializeField] int price;
    [SerializeField] string itemName;
    [SerializeField] TMP_Text buttonText;

    void Start()
    {
        buttonText.text = $"${price}";
    }

    public void BuyItem()
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