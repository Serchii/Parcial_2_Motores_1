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
        if (playerHealth == null) return;

        float percent = current / max;

        fillLifeBar.fillAmount = percent;

        Debug.Log($"Vida actual: {current}, Vida Maxima: {max}");
        
        if (percent >= 0.6)
        {
            ChangeColor(Color.green);
        }
        else if (percent >= 0.25)
        {
            ChangeColor(Color.yellow);
        }
        else
        {
            ChangeColor(Color.red);
        }
    }

    void ChangeColor(Color color)
    {
        fillLifeBar.color = color;
    }
}
