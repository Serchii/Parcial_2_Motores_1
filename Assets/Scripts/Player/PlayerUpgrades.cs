using UnityEngine;

public class PlayerUpgrades : MonoBehaviour
{
    public static PlayerUpgrades Instance { get; private set; }

    [SerializeField] private PlayerHit playerHit;
    [SerializeField] private PlayerHealth playerHealth;

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

    private void Start()
    {
        ApplyUpgrades();
    }

    public void ApplyUpgrades()
    {
        if (PlayerInventory.Instance.HasItem(ItemID.HammerUltimate))
            playerHit.SetDamage(30f);
        else if (PlayerInventory.Instance.HasItem(ItemID.HammerImproved))
            playerHit.SetDamage(20f);
        else
            playerHit.SetDamage(10f);

        if (PlayerInventory.Instance.HasItem(ItemID.HelmetUltimate))
            playerHealth.SetMaxHealthValue(200f);
        else if (PlayerInventory.Instance.HasItem(ItemID.HelmetImproved))
            playerHealth.SetMaxHealthValue(150f);
        else
            playerHealth.SetMaxHealthValue(100f);

        bool hasWatch = PlayerInventory.Instance.HasItem(ItemID.Watch);
        UIManager.Instance.ShowClockUI(hasWatch);
    }
}
