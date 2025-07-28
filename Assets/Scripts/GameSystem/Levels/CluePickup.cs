using UnityEngine;

public class CluePickup : MonoBehaviour
{
    [SerializeField] private string clueName = "Pista misteriosa";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LevelManager.Instance.CollectClue(clueName);
            Destroy(gameObject);
        }
    }
}
