using UnityEngine;

public class ClueInteract : MonoBehaviour
{
    public AudioClip interactSound;
    public Animator clueAnimator;

    private bool playerInRange = false;
    private bool collected = false;

    void Update()
    {
        if (playerInRange && !collected && Input.GetKeyDown(KeyCode.E))
        {
            collected = true;
            FindObjectOfType<LevelManager>().CollectClue();

            if (interactSound != null)
                AudioSource.PlayClipAtPoint(interactSound, transform.position);

            if (clueAnimator != null)
            {
                clueAnimator.SetTrigger("Collect");
                Destroy(gameObject, 0.5f);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}