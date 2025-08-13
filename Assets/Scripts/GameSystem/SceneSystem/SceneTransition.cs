using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] string sceneToLoad;
    [SerializeField] Vector2 spawnPositionInNextScene;
    [SerializeField] bool isDoor;

    [Header("Shop Variables")]
    [SerializeField] bool isShop;
    [SerializeField] string shopSceneName;
    [SerializeField] GameObject interactionPromptPrefab;

    private GameObject promptInstance;
    private bool canEnter = false;
    private Transform player;

    void Start()
    {
    }

    void Update()
    {
        if (isDoor && canEnter && Input.GetButtonDown("Up"))
        {
            if (isShop)
            {
                string loadLastScene;
                float x;
                float y;
                if (SceneManager.GetActiveScene().name != shopSceneName)
                {
                    PlayerPrefs.SetString("LoadLastScene", SceneManager.GetActiveScene().name);
                    PlayerPrefs.SetFloat("SpawnPosXToLoad", transform.position.x);
                    PlayerPrefs.SetFloat("SpawnPosYToLoad", transform.position.y);
                }
                else
                {
                    loadLastScene = PlayerPrefs.GetString("LoadLastScene");
                    x = PlayerPrefs.GetFloat("SpawnPosXToLoad");
                    y = PlayerPrefs.GetFloat("SpawnPosYToLoad");

                    if (loadLastScene != "")
                    {
                        sceneToLoad = loadLastScene;
                        spawnPositionInNextScene = new Vector2(x, y);
                    }
                }
                
            }
            HidePrompt();
            PlayerHealth player = FindObjectOfType<PlayerHealth>();
            if (player != null)
            {
                GameManager.Instance.SavePlayerHealth(player.Health, player.MaxHealth);
            }
            SceneLoader.Instance.LoadScene(sceneToLoad, spawnPositionInNextScene);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (!isDoor)
        {
            PlayerHealth player = FindObjectOfType<PlayerHealth>();
            if (player != null)
            {
                GameManager.Instance.SavePlayerHealth(player.Health, player.MaxHealth);
            }
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
