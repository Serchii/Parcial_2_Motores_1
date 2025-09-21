using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
    public BossHealth boss;
    public Slider healthSlider;
    [Tooltip("Separators images (opcional): colocar 2 imágenes para marcar los límites sobre la barra.")]
    public RectTransform[] separators;

    private void Start()
    {
        if (boss == null) boss = FindObjectOfType<BossHealth>();
        if (boss == null) return;

        healthSlider.maxValue = boss.GetMaxHealth();
        healthSlider.value = boss.GetCurrentHealth();
        boss.OnHealthChanged.AddListener(OnHealthChanged);

        UpdateSeparatorsPositions();
    }

    private void OnHealthChanged(int currentHealth)
    {
        healthSlider.value = currentHealth;
    }

    private void UpdateSeparatorsPositions()
    {
        if (separators == null || separators.Length == 0 || healthSlider == null || boss == null) return;
        int[] thresholds = boss.phaseThresholds;
        if (thresholds == null || thresholds.Length < 2) return;

        RectTransform fillArea = healthSlider.fillRect;
        RectTransform sliderRect = healthSlider.GetComponent<RectTransform>();

        for (int i = 0; i < separators.Length && i < thresholds.Length - 0; i++)
        {
            float normalized = (float)thresholds[i] / boss.GetMaxHealth();
            float xLocal = Mathf.Lerp(-sliderRect.rect.width * 0.5f, sliderRect.rect.width * 0.5f, normalized);
            separators[i].anchoredPosition = new Vector2(xLocal, separators[i].anchoredPosition.y);
        }
    }
}
