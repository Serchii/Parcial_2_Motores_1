using UnityEngine;

public class CluePickup : MonoBehaviour
{
    [SerializeField] private string clueName = "Pista misteriosa";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.CollectClue(clueName);
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("LevelManager.Instance es null. Verifica que se haya inicializado correctamente.");
        }
    }
}
