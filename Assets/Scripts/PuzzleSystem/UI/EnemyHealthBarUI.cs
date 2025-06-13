using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarUI : MonoBehaviour
{
    [SerializeField] Image healthFill;
    [SerializeField] float visibleDuration = 1.5f;

    private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 1f, 0);
    private float timer;
    private bool isVisible = false;

    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
    }

    public void SetHealth(float current, float max)
    {
        healthFill.fillAmount = current / max;
        Show();

        if (current <= 0)
        {
            Destroy(gameObject, 1f);
        }
    }

    private void Show()
    {
        if (!isVisible)
        {
            gameObject.SetActive(true);
            isVisible = true;
        }
        timer = visibleDuration;
    }

    void Update()
    {
        if (target == null) return;

        // Seguir al objetivo
        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position + offset);
        transform.position = screenPos;

        if (isVisible)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                gameObject.SetActive(false);
                isVisible = false;
            }
        }
    }
}
