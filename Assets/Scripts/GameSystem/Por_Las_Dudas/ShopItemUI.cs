using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Image icon;
    [SerializeField] private Button buyButton;

    private ShopItem item;

    public void Setup(ShopItem newItem)
    {
        item = newItem;

        nameText.text = item.itemName;
        descriptionText.text = item.description;
        priceText.text = "$" + item.price;
        icon.sprite = item.icon;

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(BuyItem);
    }

    private void BuyItem()
    {
        if (GameManager.Instance.SpendMoney(item.price))
        {
            PlayerInventory.Instance.BuyItem(item.itemId);
            buyButton.interactable = false;
            priceText.text = "Comprado";

            Debug.Log($"Compraste {item.itemName}");
        }
        else
        {
            Debug.Log("Dinero insuficiente");
        }
    }
}