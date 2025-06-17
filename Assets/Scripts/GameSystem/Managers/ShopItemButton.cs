using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemButton : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private Image _icon;
    [SerializeField] private Button _buyButton;

    private ShopItem _item;
    private ShopManager _shopManager;

    public void Setup(ShopItem item, ShopManager manager)
    {
        _item = item;
        _shopManager = manager;

        _nameText.text = item.itemName;
        _descriptionText.text = item.description;
        _priceText.text = $"${item.price}";
        _icon.sprite = item.icon;

        _buyButton.onClick.RemoveAllListeners();
        _buyButton.onClick.AddListener(() => _shopManager.TryBuyItem(_item, this));
    }

    public void DisableButton()
    {
        _buyButton.interactable = false;
        _priceText.text = "Comprado";
    }
}
