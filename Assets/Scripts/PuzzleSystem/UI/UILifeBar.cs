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

        if (playerHealth != null) 
        {
            playerHealth.OnHealthChanged += UpdateLifeBar;

            UpdateLifeBar(playerHealth.Health,playerHealth.MaxHealth);
        }
    }

    void UpdateLifeBar(float current, float max)
    {
        if(playerHealth == null) return;

        fillLifeBar.fillAmount = current / max;
    }
}
