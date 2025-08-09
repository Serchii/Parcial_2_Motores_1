using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Localization;
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

    [Header("Traduccion")]
    [SerializeField] string titleTranslate;
    [SerializeField] string descriptionTranslate;
    [SerializeField] string tableName = "Tutorial";

    [Header("Referencia Player")]
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerAttack playerAttack;

    [Header("Miniatura Settings")]
    [Range(0.1f, 1f)]
    public float thumbnailScale = 0.3f;
    public RectTransform imageRect;

    private int currentStep = 0;

    void Start()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);

        if (playerAttack == null)
            playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();


        if (playerMovement == null)
            playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    public void OpenTutorial()
    {
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(true);
            currentStep = 0;
            ShowStep();
            SetMovePlayer(false);
            //Time.timeScale = 0f;
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


        SetMovePlayer(true);
        Time.timeScale = 1f;
    }

    public void OnClickNext()
    {
        if (currentStep < tutorialData.steps.Length - 1)
        {
            currentStep++;
            ShowStep();
        }
        else
        {
            tutorialPanel.GetComponent<Animator>().SetTrigger("MoveOut");
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
        bool isNotLastStep;
        if (tutorialData != null && tutorialData.steps.Length > 0)
        {
            //titleText.text = tutorialData.steps[currentStep].tutorialTitle;
            tutorialImageUI.sprite = tutorialData.steps[currentStep].tutorialImage;
            //descriptionText.text = tutorialData.steps[currentStep].tutorialDescription;
            StartCoroutine(GetTranslateText(tutorialData.steps[currentStep].tutorialTitle, tutorialData.steps[currentStep].tutorialDescription));

            AdjustImageSize();

            prevButton.interactable = currentStep > 0;
            isNotLastStep = currentStep < tutorialData.steps.Length - 1;

            if (isNotLastStep)
            {
                nextButton.GetComponentInChildren<TMP_Text>().text = "Next";
            }
            else
            {
                nextButton.GetComponentInChildren<TMP_Text>().text = "Close";
            }
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

    void SetMovePlayer(bool value)
    {
        playerMovement.SetCanMove(value);
        playerAttack.SetCanAttack(value);
    }
    
    IEnumerator GetTranslateText(string titleKey, string descriptionKey)
    {
        //Traducimos titulo de tutorial
        titleTranslate = string.Empty;
        var localizedLine = new LocalizedString(tableName, titleKey);
        var handle = localizedLine.GetLocalizedStringAsync();
        yield return handle;

        titleTranslate = handle.Result;

        //traducimos descripcion del tutorial
        descriptionTranslate = string.Empty;
        localizedLine = new LocalizedString(tableName, descriptionKey);
        handle = localizedLine.GetLocalizedStringAsync();
        yield return handle;

        descriptionTranslate = handle.Result;

        var textComponent = titleText;
        if (textComponent != null)
        {
            textComponent.text = titleTranslate;
        }

        textComponent = descriptionText;
        if (textComponent != null)
        {
            textComponent.text = descriptionTranslate;
        }
    }
}
