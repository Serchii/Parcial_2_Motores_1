using UnityEngine;
using UnityEngine.UI;

public class UILifeBar : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] Image fillLifeBar;
    [SerializeField] PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerHealth>();
        float health = GameManager.Instance.SavedHealth;
        float maxHealth = GameManager.Instance.SavedMaxHealth;

        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += UpdateLifeBar;

            UpdateLifeBar(health, maxHealth);
        }
    }

    void UpdateLifeBar(float current, float max)
    {
        if(playerHealth == null) return;

        fillLifeBar.fillAmount = current / max;
    }
}
