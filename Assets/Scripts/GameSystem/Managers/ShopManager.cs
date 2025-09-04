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
        if (_itemsParent == null || _shopItemButtonPrefab == null)
        {
            Debug.LogError("⚠️ ShopManager: faltan referencias en el inspector.");
            return;
        }

        foreach (Transform child in _itemsParent)
        {
            Destroy(child.gameObject);
        }

        foreach (ShopItem item in _itemsForSale)
        {
            SpawnNewItem(item);
        }
    }

    private void SpawnNewItem(ShopItem item)
    {
        GameObject buttonObj = Instantiate(_shopItemButtonPrefab, _itemsParent);
        buttonObj.SetActive(true);

        ShopItemButton button = buttonObj.GetComponent<ShopItemButton>();
        if (button == null)
        {
            Debug.LogError("⚠️ El prefab no tiene ShopItemButton.");
            return;
        }

        button.Setup(item, this);
    }

    public void TryBuyItem(ShopItem item, ShopItemButton button)
    {
        if (item == null || button == null)
        {
            Debug.LogError("⚠️ ShopManager: item o botón es null.");
            return;
        }

        if (GameManager.Instance == null || PlayerInventory.Instance == null)
        {
            Debug.LogError("⚠️ GameManager o PlayerInventory no existen.");
            return;
        }

        if (PlayerInventory.Instance.HasItem(item.itemId))
        {
            Debug.Log("Ya tienes este ítem: " + item.itemName);
            return;
        }

        if (!GameManager.Instance.SpendMoney(item.price))
        {
            Debug.Log("⚠️ Dinero insuficiente para: " + item.itemName);
            return;
        }

        PlayerInventory.Instance.BuyItem(item.itemId);

        if (item.nextUpgrade != null)
        {
            button.Setup(item.nextUpgrade, this);
        }
        else
        {
            button.DisableButton();
        }

        Debug.Log("✅ Comprado: " + item.itemName);
    }
}
