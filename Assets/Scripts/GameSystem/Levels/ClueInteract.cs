using UnityEngine;

public class ClueInteract : MonoBehaviour
{
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private string clueName = "Pista misteriosa";

    private bool isPlayerNearby = false;

    private void Update()
    {
        if (!isPlayerNearby) return;

        if (Input.GetKeyDown(interactKey))
        {
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.CollectClue(clueName);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("LevelManager.Instance es null. Verifica que esté correctamente inicializado en la escena inicial.");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }
}
