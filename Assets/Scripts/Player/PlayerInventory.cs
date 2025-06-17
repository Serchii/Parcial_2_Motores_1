using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;
    private Dictionary<ItemID, bool> items = new();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeInventory();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeInventory()
    {
        foreach (ItemID item in Enum.GetValues(typeof(ItemID)))
            items[item] = false;
    }

    public void BuyItem(ItemID item)
    {
        items[item] = true;

        if (item == ItemID.Watch && UIManager.Instance != null)
            UIManager.Instance.ShowClockUI(true);

        SaveData();
        OnItemPurchased?.Invoke(item);
    }

    public bool HasItem(ItemID item) => items.TryGetValue(item, out bool owned) && owned;

    public void SaveData()
    {
        foreach (var kvp in items)
            PlayerPrefs.SetInt($"Item_{kvp.Key}", kvp.Value ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void LoadData()
    {
        foreach (ItemID item in Enum.GetValues(typeof(ItemID)))
        {
            bool owned = PlayerPrefs.GetInt($"Item_{item}", 0) == 1;
            items[item] = owned;

            if (item == ItemID.Watch && owned && UIManager.Instance != null)
                UIManager.Instance.ShowClockUI(true);
        }
    }

    public static event Action<ItemID> OnItemPurchased;
}