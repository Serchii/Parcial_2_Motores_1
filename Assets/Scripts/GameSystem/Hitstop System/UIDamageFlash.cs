using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIDamageFlash : MonoBehaviour
{
    [SerializeField] private Image uiImage;
    [SerializeField] private Material flashMaterial;
    [SerializeField] private float flashDuration = 0.1f;
    [SerializeField] PlayerHealth playerHealth;

    private Material originalMaterial;
    private Coroutine flashRoutine;

    void Awake()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerHealth>();

        if (uiImage == null)
            uiImage = GetComponent<Image>();

        if (uiImage != null)
            originalMaterial = uiImage.material;

        if (playerHealth != null)
        {
            playerHealth.OnFlash += Flash;
        }
    }

    public void Flash()
    {
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        uiImage.material = flashMaterial;

        yield return new WaitForSecondsRealtime(flashDuration); // Funciona con TimeScale = 0

        uiImage.material = originalMaterial;
    }
}
