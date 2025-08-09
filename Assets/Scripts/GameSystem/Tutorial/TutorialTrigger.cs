using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public TutorialUIManager tutorialUI;
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            tutorialUI.OpenTutorialFromTrigger();
        }
    }
}
