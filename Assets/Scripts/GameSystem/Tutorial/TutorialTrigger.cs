using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public TutorialUIManager tutorialUI;
    //TutorialData
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            //TutorialData
            tutorialUI.OpenTutorialFromTrigger();
        }
    }
}
