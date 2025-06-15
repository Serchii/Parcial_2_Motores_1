using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Item")]
public class ShopItem : ScriptableObject
{
    public ItemID itemId;
    public string itemName;
    public string description;
    public int price;
    public Sprite icon;
    public ShopItem nextUpgrade;
}