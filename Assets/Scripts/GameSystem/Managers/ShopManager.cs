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
        foreach (Transform child in _itemsParent)
        {
            Destroy(child.gameObject);
        }

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
        buttonObj.SetActive(true); // Asegurate que se active

        ShopItemButton button = buttonObj.GetComponent<ShopItemButton>();
        if (button == null)
        {
            Debug.LogError("No se encontró ShopItemButton en el prefab.");
            return;
        }

        button.Setup(item, this);
    }
    private void ReplaceItemButton(ShopItem nextItem, ShopItemButton oldButton)
    {
        GameObject newButtonObj = Instantiate(_shopItemButtonPrefab, _itemsParent);

        int index = oldButton.transform.GetSiblingIndex();
        newButtonObj.transform.SetSiblingIndex(index);

        Destroy(oldButton.gameObject);

        ShopItemButton newButton = newButtonObj.GetComponent<ShopItemButton>();
        newButton.Setup(nextItem, this);
    }

    public void TryBuyItem(ShopItem item, ShopItemButton button)
    {
        if (GameManager.Instance.SpendMoney(item.price))
        {
            PlayerInventory.Instance.BuyItem(item.itemId);

            PlayerUpgrades.Instance.ApplyUpgrades();

            if (item.nextUpgrade != null)
            {
                ReplaceItemButton(item.nextUpgrade, button);
            }
            else
            {
                button.DisableButton();
            }
        }
        else
        {
            Debug.Log("Dinero insuficiente.");
        }
    }

}
