using System.Collections.Generic;
using UnityEngine;

public enum ItemID
{
    Watch,
    ExpertCertificate
}

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    private Dictionary<ItemID, bool> items = new Dictionary<ItemID, bool>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeInventory();
            LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnApplicationQuit()
    {
        SaveData();
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
            SaveData();
    }

    private void InitializeInventory()
    {
        foreach (ItemID item in System.Enum.GetValues(typeof(ItemID)))
        {
            items[item] = false;
        }
    }

    public void BuyItem(ItemID item)
    {
        items[item] = true;

        if (item == ItemID.Watch && UIManager.Instance != null)
            UIManager.Instance.ShowClockUI(true);

        SaveData();
    }

    public bool HasItem(ItemID item)
    {
        return items.ContainsKey(item) && items[item];
    }

    private void SaveData()
    {
        foreach (var kvp in items)
        {
            PlayerPrefs.SetInt($"Item_{kvp.Key}", kvp.Value ? 1 : 0);
        }

        PlayerPrefs.Save();
    }

    private void LoadData()
    {
        foreach (ItemID item in System.Enum.GetValues(typeof(ItemID)))
        {
            bool owned = PlayerPrefs.GetInt($"Item_{item}", 0) == 1;
            items[item] = owned;

            if (item == ItemID.Watch && owned && UIManager.Instance != null)
                UIManager.Instance.ShowClockUI(true);
        }
    }
}
