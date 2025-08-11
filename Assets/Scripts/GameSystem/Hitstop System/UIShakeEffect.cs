using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIShakeEffect : MonoBehaviour
{
    [SerializeField] private float shakeAmount = 5f; 
    [SerializeField] private float shakeDuration = 0.1f;
    [SerializeField] PlayerHealth playerHealth;

    private Vector3 originalPosition;
    private Coroutine shakeCoroutine;

    void Awake()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerHealth>();
        originalPosition = transform.localPosition;

        if (playerHealth != null)
        {
            playerHealth.OnShake += Shake;
        }
    }

    public void Shake()
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        
        gameObject.GetComponent<Animator>().SetTrigger("Hurt");
        shakeCoroutine = StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeAmount;
            float y = Random.Range(-1f, 1f) * shakeAmount;

            transform.localPosition = originalPosition + new Vector3(x, y, 0f);

            elapsed += Time.unscaledDeltaTime; // Funciona con TimeScale = 0
            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}
