using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPanelAnimator : MonoBehaviour
{
    [SerializeField] TutorialUIManager tutorialManager;

    public void FinishTutorial()
    {
        tutorialManager.OnClickCloseTutorial();
    }
}
