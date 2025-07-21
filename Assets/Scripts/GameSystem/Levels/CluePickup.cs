using UnityEngine;

public class CluePickup : MonoBehaviour
{
    public AudioClip pickupSound;
    public Animator clueAnimator;

    private bool collected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected || !other.CompareTag("Player")) return;

        collected = true;
        FindObjectOfType<LevelManager>().CollectClue();

        if (pickupSound != null)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);

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
