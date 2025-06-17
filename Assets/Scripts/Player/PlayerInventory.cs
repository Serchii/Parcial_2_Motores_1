using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    private HashSet<ItemID> _ownedItems = new HashSet<ItemID>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadInventory();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool HasItem(ItemID id) => _ownedItems.Contains(id);

    public void BuyItem(ItemID id)
    {
        _ownedItems.Add(id);
        SaveInventory();
    }

    public void ResetInventory()
    {
        _ownedItems.Clear();
        SaveInventory();
    }

    private void SaveInventory()
    {
        string saved = string.Join(",", _ownedItems);
        PlayerPrefs.SetString("Inventory_ItemIDs", saved);
    }

    private void LoadInventory()
    {
        string saved = PlayerPrefs.GetString("Inventory_ItemIDs", "");
        _ownedItems = new HashSet<ItemID>();

        foreach (string s in saved.Split(',', StringSplitOptions.RemoveEmptyEntries))
        {
            if (Enum.TryParse(s, out ItemID id))
            {
                _ownedItems.Add(id);
            }
        }
    }
}
