using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialUIManager : MonoBehaviour
{
    [Header("Datos del Tutorial")]
    public TutorialData tutorialData;

    [Header("UI Panel")]
    public GameObject tutorialPanel;

    [Header("UI Elements (TextMeshPro)")]
    public TMP_Text titleText;
    public Image tutorialImageUI;
    public TMP_Text descriptionText;
    public Button prevButton;
    public Button nextButton;

    [Header("Miniatura Settings")]
    [Range(0.1f, 1f)]
    public float thumbnailScale = 0.3f;
    public RectTransform imageRect;

    private int currentStep = 0;

    void Start()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);
    }

    public void OpenTutorial()
    {
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(true);
            currentStep = 0;
            ShowStep();
            Time.timeScale = 0f;
        }
    }

    public void OpenTutorialFromTrigger()
    {
        OpenTutorial();
    }

    public void OnClickCloseTutorial()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void OnClickNext()
    {
        if (currentStep < tutorialData.steps.Length - 1)
        {
            currentStep++;
            ShowStep();
        }
    }

    public void OnClickPrevious()
    {
        if (currentStep > 0)
        {
            currentStep--;
            ShowStep();
        }
    }

    private void ShowStep()
    {
        if (tutorialData != null && tutorialData.steps.Length > 0)
        {
            titleText.text = tutorialData.steps[currentStep].tutorialTitle;
            tutorialImageUI.sprite = tutorialData.steps[currentStep].tutorialImage;
            descriptionText.text = tutorialData.steps[currentStep].tutorialDescription;

            AdjustImageSize();

            prevButton.interactable = currentStep > 0;
            nextButton.interactable = currentStep < tutorialData.steps.Length - 1;
        }
    }

    private void AdjustImageSize()
    {
        if (tutorialImageUI.sprite == null || imageRect == null) return;

        float originalWidth = tutorialImageUI.sprite.texture.width;
        float originalHeight = tutorialImageUI.sprite.texture.height;

        float aspectRatio = originalWidth / originalHeight;

        float targetWidth = Screen.width * thumbnailScale;
        float targetHeight = targetWidth / aspectRatio;

        imageRect.sizeDelta = new Vector2(targetWidth, targetHeight);
    }
}
