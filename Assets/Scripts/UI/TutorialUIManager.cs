using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialUIManager : MonoBehaviour
{
    public GameObject tutorialPromptPanel;
    public GameObject tutorialInfoPanel;
    public string tutorialSceneName = "TutorialScene";

    void Start()
    {
        if (SceneManager.GetActiveScene().name == tutorialSceneName)
        {
            tutorialPromptPanel.SetActive(true);
            tutorialInfoPanel.SetActive(false);
            Time.timeScale = 0f;
        }
        else
        {
            tutorialPromptPanel.SetActive(false);
            tutorialInfoPanel.SetActive(false);
        }
    }

    public void OnClickYes()
    {
        tutorialPromptPanel.SetActive(false);
        tutorialInfoPanel.SetActive(true);
    }

    public void OnClickNo()
    {
        tutorialPromptPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnClickCloseTutorial()
    {
        tutorialInfoPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
