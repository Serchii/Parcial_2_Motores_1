using UnityEngine;

public class ClueInteract : MonoBehaviour
{
    [SerializeField] private string clueId = "clue_default";
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private bool isPlayerInRange = false;

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(interactKey))
        {
            Debug.Log($"Presionaste {interactKey} para recoger pista: {clueId}");

            if (LevelManagerTemp.Instance != null)
            {
                LevelManagerTemp.Instance.CollectClue(clueId);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("LevelManagerTemp.Instance es null");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Jugador entró al trigger 2D de la pista: " + clueId);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log("Jugador salió del trigger 2D de la pista: " + clueId);
        }
    }
}
