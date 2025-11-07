using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Item")]
public class ShopItem : ScriptableObject
{
    public ItemID itemId;
    
    [Header("Localization")]
    public string itemNameKey; // Clave para la traducción del nombre
    public string descriptionKey; // Clave para la traducción de la descripción

    [Header("Item Details")]
    public int price;
    public Sprite icon;
    public ShopItem nextUpgrade;
}
