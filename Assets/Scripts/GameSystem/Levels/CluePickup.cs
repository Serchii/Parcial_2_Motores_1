using UnityEngine;

public class CluePickup : MonoBehaviour
{
    [SerializeField] private string clueName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LevelManagerTemp manager = FindObjectOfType<LevelManagerTemp>();
            if (manager != null)
            {
                manager.CollectClue(clueName);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("CluePickup: No se encontró LevelManagerTemp en la escena.");
            }
        }
    }
}
