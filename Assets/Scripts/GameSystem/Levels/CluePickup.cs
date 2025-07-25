using UnityEngine;

public class CluePickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LevelManager.Instance.CollectClue();
            Destroy(gameObject);
        }
    }
}
