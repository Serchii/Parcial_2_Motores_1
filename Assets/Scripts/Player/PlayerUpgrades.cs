using UnityEngine;

public class PlayerUpgrades : MonoBehaviour
{
    public static PlayerUpgrades Instance { get; private set; }

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ApplyUpgrades()
    {
        bool hasWatch = PlayerInventory.Instance.HasItem(ItemID.Watch);
        UIManager.Instance.ShowClockUI(hasWatch);
    }
}
