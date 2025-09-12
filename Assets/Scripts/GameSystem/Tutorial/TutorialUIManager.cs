using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Localization;
using TMPro;

public class TutorialUIManager : MonoBehaviour
{
    [Header("Datos del Tutorial")]
    public TutorialData tutorialData;
    [SerializeField] private string resourcesFolder = "TutorialData";

    [Header("UI Panel")]
    public GameObject tutorialPanel;

    [Header("UI Elements (TextMeshPro)")]
    public TMP_Text titleText;
    public Image tutorialImageUI;
    public TMP_Text descriptionText;
    public Button prevButton;
    public Button nextButton;

    [Header("Traducci�n")]
    [SerializeField] string titleTranslate;
    [SerializeField] string descriptionTranslate;
    [SerializeField] string tableName = "Tutorial";
    [SerializeField] string nextTranslate;
    [SerializeField] string closeTranslate;
    [SerializeField] string nextKey;
    [SerializeField] string closeKey;

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
        LoadTutorialDataByScene();

        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);

        EnsurePlayerReferences();

        StartCoroutine(GetTranslateButton(closeKey, nextKey, "UI"));
    }

    void LoadTutorialDataByScene()
    {
        if (tutorialData != null) return;

        string sceneName = SceneManager.GetActiveScene().name;
        string fileName = $"NewTutorialData_{sceneName}"; 

        tutorialData = Resources.Load<TutorialData>($"{resourcesFolder}/{fileName}");

        if (tutorialData == null)
            Debug.LogError($"No se encontr� TutorialData en Resources/{resourcesFolder} con el nombre {fileName}");
    }

    void EnsurePlayerReferences()
    {
        if (playerAttack == null || playerMovement == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                if (playerAttack == null) playerAttack = player.GetComponent<PlayerAttack>();
                if (playerMovement == null) playerMovement = player.GetComponent<PlayerMovement>();
            }
        }
    }

    public void OpenTutorial()
    {
        EnsurePlayerReferences();

        if (tutorialPanel != null && tutorialData != null && tutorialData.steps.Length > 0)
        {
            tutorialPanel.SetActive(true);
            currentStep = 0;
            ShowStep();
            GameStateManager.Instance.EnterTutorial();
            Time.timeScale = 0f;
        }
        else
        {
            Debug.LogError("No se puede abrir el tutorial: referencias faltantes o sin pasos.");
        }
    }

    public void OpenTutorialFromTrigger() => OpenTutorial();

    public void OnClickCloseTutorial()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);

        GameStateManager.Instance.ExitTutorial();
        Time.timeScale = 1f;
    }

    public void OnClickNext()
    {
        if (tutorialData == null || tutorialData.steps.Length == 0) return;

        if (currentStep < tutorialData.steps.Length - 1)
        {
            currentStep++;
            ShowStep();
        }
        else
        {
            if (tutorialPanel != null && tutorialPanel.GetComponent<Animator>() != null)
                tutorialPanel.GetComponent<Animator>().SetTrigger("MoveOut");
            else
                OnClickCloseTutorial();
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
        if (tutorialData == null || tutorialData.steps.Length == 0) return;

        tutorialImageUI.sprite = tutorialData.steps[currentStep].tutorialImage;
        StartCoroutine(GetTranslateText(tutorialData.steps[currentStep].tutorialTitle, tutorialData.steps[currentStep].tutorialDescription, tableName));

        AdjustImageSize();

        if (prevButton != null)
            prevButton.interactable = currentStep > 0;

        if (nextButton != null)
        {
            bool isNotLastStep = currentStep < tutorialData.steps.Length - 1;
            TMP_Text nextButtonText = nextButton.GetComponentInChildren<TMP_Text>();
            if (nextButtonText != null)
                nextButtonText.text = isNotLastStep ? nextTranslate : closeTranslate;
        }
    }

    private void AdjustImageSize()
    {
        if (tutorialImageUI == null || tutorialImageUI.sprite == null || imageRect == null) return;

        float originalWidth = tutorialImageUI.sprite.texture.width;
        float originalHeight = tutorialImageUI.sprite.texture.height;
        float aspectRatio = originalWidth / originalHeight;

        float targetWidth = Screen.width * thumbnailScale;
        float targetHeight = targetWidth / aspectRatio;

        imageRect.sizeDelta = new Vector2(targetWidth, targetHeight);
    }

    void SetMovePlayer(bool value)
    {
        if (playerMovement != null)
            playerMovement.SetCanMove(value);

        if (playerAttack != null)
            playerAttack.SetCanAttack(value);
    }

    IEnumerator GetTranslateText(string titleKey, string descriptionKey, string tableName)
    {
        if (titleText == null || descriptionText == null) yield break;

        titleTranslate = string.Empty;
        var localizedLine = new LocalizedString(tableName, titleKey);
        var handle = localizedLine.GetLocalizedStringAsync();
        yield return handle;
        titleTranslate = handle.Result;

        descriptionTranslate = string.Empty;
        localizedLine = new LocalizedString(tableName, descriptionKey);
        handle = localizedLine.GetLocalizedStringAsync();
        yield return handle;
        descriptionTranslate = handle.Result;

        titleText.text = titleTranslate;
        descriptionText.text = descriptionTranslate;
    }

    IEnumerator GetTranslateButton(string closeKey, string NextKey, string tableName)
    {
        if (titleText == null || descriptionText == null) yield break;

        closeTranslate = string.Empty;
        var localizedLine = new LocalizedString(tableName, closeKey);
        var handle = localizedLine.GetLocalizedStringAsync();
        yield return handle;
        closeTranslate = handle.Result;

        nextTranslate = string.Empty;
        localizedLine = new LocalizedString(tableName, NextKey);
        handle = localizedLine.GetLocalizedStringAsync();
        yield return handle;
        nextTranslate = handle.Result;

    }
}
