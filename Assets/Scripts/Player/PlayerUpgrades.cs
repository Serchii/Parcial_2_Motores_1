using UnityEngine;

public class PlayerUpgrades : MonoBehaviour
{
    public static PlayerUpgrades Instance { get; private set; }

    [SerializeField] private PlayerAttack playerAttack;
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
        if (playerAttack == null)
            playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();

        if (playerHealth == null)
            playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();

        ApplyUpgrades();
    }

    public void ApplyUpgrades()
    {
        if (PlayerInventory.Instance.HasItem(ItemID.HammerUltimate))
            playerAttack.SetAttackDamage(30f);
        else if (PlayerInventory.Instance.HasItem(ItemID.HammerImproved))
            playerAttack.SetAttackDamage(20f);
        else
            playerAttack.SetAttackDamage(10f);

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
