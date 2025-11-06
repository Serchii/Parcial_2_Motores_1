using UnityEngine;
using TMPro;
using System.Collections;

public class EnemyTextDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float displayTime = 1.5f;
    [SerializeField] private float fadeTime = 1f;

    private Transform target;
    private Camera cam;

    void Awake()
    {
        Debug.Log("EnemyTextDisplay: Awake llamado.");
        if (textMesh == null)
        {
            textMesh = GetComponentInChildren<TextMeshProUGUI>();
            if (textMesh == null)
            {
                Debug.LogError("EnemyTextDisplay: TextMeshProUGUI no encontrado en Awake. Asegúrese de que el prefab tenga un TextMeshProUGUI como hijo o en el mismo GameObject.");
            }
        }
        if (textMesh != null)
        {
            textMesh.gameObject.SetActive(false); // Start inactive
            Debug.Log("EnemyTextDisplay: TextMeshProUGUI encontrado y desactivado inicialmente.");
        }
    }

    void Start()
    {
        cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("No se encontró ninguna cámara con el tag 'MainCamera'");
        }
    }

    void Update()
    {
        if (target != null && cam != null)
        {
            Vector3 screenPos = cam.WorldToScreenPoint(target.position + offset);
            transform.position = screenPos;
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void ShowText(string message)
    {
        Debug.Log($"EnemyTextDisplay: ShowText llamado con mensaje: '{message}'");
        if (textMesh == null)
        {
            Debug.LogWarning("EnemyTextDisplay: TextMeshProUGUI component not found. No se puede mostrar el texto.");
            return;
        }

        textMesh.text = message;
        Debug.Log($"EnemyTextDisplay: Texto asignado a textMesh: '{textMesh.text}'");
        StartCoroutine(FadeTextAndDeactivate());
        Debug.Log("EnemyTextDisplay: Coroutine FadeTextAndDeactivate iniciada.");
    }

    private IEnumerator FadeTextAndDeactivate()
    {
        Debug.Log("EnemyTextDisplay: FadeTextAndDeactivate Coroutine iniciada. Activando GameObject del texto.");
        textMesh.gameObject.SetActive(true);
        Color startColor = textMesh.color;
        startColor.a = 1f; // Ensure text is fully visible at the start
        textMesh.color = startColor;
        Debug.Log($"EnemyTextDisplay: Texto visible. Esperando {displayTime} segundos.");

        yield return new WaitForSeconds(displayTime);
        Debug.Log($"EnemyTextDisplay: Tiempo de visualización terminado. Iniciando desvanecimiento durante {fadeTime} segundos.");

        float timer = 0f;
        Color currentColor = textMesh.color;

        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            currentColor.a = Mathf.Lerp(1f, 0f, timer / fadeTime);
            textMesh.color = currentColor;
            yield return null;
        }

        textMesh.gameObject.SetActive(false);
        // Reset alpha for next time, but keep it transparent if it's meant to be hidden
        currentColor.a = 0f;
        textMesh.color = currentColor;
        Debug.Log("EnemyTextDisplay: Texto desactivado y GameObject destruido.");
        Destroy(gameObject); // Destroy the text instance after it fades
    }
}
