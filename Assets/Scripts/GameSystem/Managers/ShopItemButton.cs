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
        if (item == null || manager == null)
        {
            Debug.LogError("⚠️ ShopItemButton.Setup recibió parámetros nulos.");
            return;
        }

        _item = item;
        _shopManager = manager;

        if (_nameText != null) _nameText.text = item.itemName;
        if (_descriptionText != null) _descriptionText.text = item.description;
        if (_priceText != null) _priceText.text = $"${item.price}";
        if (_icon != null) _icon.sprite = item.icon;

        if (_buyButton != null)
        {
            _buyButton.onClick.RemoveAllListeners();
            _buyButton.onClick.AddListener(() => _shopManager.TryBuyItem(_item, this));
            _buyButton.interactable = true;
        }
        else
        {
            Debug.LogError("⚠️ No se asignó el botón en el prefab de ShopItemButton.");
        }

        if (PlayerInventory.Instance != null && PlayerInventory.Instance.HasItem(item.itemId))
        {
            DisableButton();
        }
    }

    public void DisableButton()
    {
        if (_buyButton != null)
            _buyButton.interactable = false;

        if (_priceText != null)
            _priceText.text = "Comprado";

        if (_nameText != null)
            _nameText.color = Color.gray;
    }
}
