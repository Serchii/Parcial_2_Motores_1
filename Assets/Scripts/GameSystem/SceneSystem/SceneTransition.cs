using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] string sceneToLoad;
    [SerializeField] Vector2 spawnPositionInNextScene;
    [SerializeField] bool isDoor;
    [SerializeField] GameObject interactionPromptPrefab;
    [SerializeField] GameObject puzzlePrefab;

    private GameObject promptInstance;
    private bool canEnter = false;
    private Transform player;

    void Start()
    {
        puzzlePrefab = GameObject.FindGameObjectWithTag("PuzzleLevel");
    }

    void Update()
    {
        if (isDoor && canEnter && Input.GetButtonDown("Up"))
        {
            HidePrompt();
            SceneLoader.Instance.LoadScene(sceneToLoad, spawnPositionInNextScene);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (!isDoor)
        {
            SceneLoader.Instance.LoadScene(sceneToLoad, spawnPositionInNextScene);
        }
        else
        {
            canEnter = true;
            player = other.transform;
            ShowPrompt();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isDoor)
        {
            canEnter = false;
            HidePrompt();
        }
    }

    private void ShowPrompt()
    {
        if (interactionPromptPrefab != null && promptInstance == null)
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            promptInstance = Instantiate(interactionPromptPrefab, canvas.transform);

            UIFollowPlayer followScript = promptInstance.GetComponent<UIFollowPlayer>();
            followScript?.SetTarget(player);
        }
    }

    private void HidePrompt()
    {
        if (promptInstance != null)
        {
            Destroy(promptInstance);
            promptInstance = null;
        }
    }
}
