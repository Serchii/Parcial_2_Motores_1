using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private Transform _itemsParent;
    [SerializeField] private GameObject _shopItemButtonPrefab;
    [SerializeField] private ShopItem[] _itemsForSale;

    private void Start()
    {
        PopulateShop();
    }

    private void PopulateShop()
    {
        foreach (ShopItem item in _itemsForSale)
        {
            if (!PlayerInventory.Instance.HasItem(item.itemId))
            {
                SpawnNewItem(item);
            }
        }
    }

    public void SpawnNewItem(ShopItem item)
    {
        GameObject buttonObj = Instantiate(_shopItemButtonPrefab, _itemsParent);
        ShopItemButton button = buttonObj.GetComponent<ShopItemButton>();
        button.Setup(item, this);
    }

    public void TryBuyItem(ShopItem item, ShopItemButton button)
    {
        if (GameManager.Instance.SpendMoney(item.price))
        {
            PlayerInventory.Instance.BuyItem(item.itemId);
            button.DisableButton();

            if (item.nextUpgrade != null)
            {
                SpawnNewItem(item.nextUpgrade);
            }
        }
        else
        {
            Debug.Log("Dinero insuficiente.");
        }
    }
}