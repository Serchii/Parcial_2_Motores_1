/* using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIPauseManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject resumeButton;
    [SerializeField] TMP_Text title;
    [SerializeField] GameState currentGameState;

    private bool isPaused = false;

    private void OnEnable()
    {
        GameManager.OnGameEnded += ShowEndScreen;
        GameSceneManager.OnSceneFullyLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        GameManager.OnGameEnded -= ShowEndScreen;
        GameSceneManager.OnSceneFullyLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause") && !GameManager.Instance.IsGameOver())
        {
            currentGameState = GameStateManager.Instance.CurrentGameState;
            if (currentGameState != GameState.Gameplay && currentGameState != GameState.Paused)
                return;

            GameState newGameState = currentGameState == GameState.Gameplay ? GameState.Paused : GameState.Gameplay;

            GameStateManager.Instance.SetState(newGameState);

            if (newGameState == GameState.Paused)
                PauseGame();
            else
                ResumeGame();
        }
    }

    public void PauseGame()
    {
        GameStateManager.Instance.SetState(GameState.Paused);
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        title.text = "PAUSED";
    }

    public void ResumeGame()
    {
        GameStateManager.Instance.SetState(GameState.Gameplay);
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    public void RestartGame()
    {
        ResumeGame();
        GameManager.Instance.SetMaxHealth();
        StartCoroutine(GameSceneManager.Instance.LoadSceneWithTransitionRoutine(SceneManager.GetActiveScene().name));
    }

    public void ReturnToMenu()
    {
        ResumeGame();
        StartCoroutine(GameSceneManager.Instance.LoadSceneWithTransitionRoutine("MainMenu"));
    }

    public void ShowEndScreen(bool won, string text)
    {
        GameStateManager.Instance.SetState(GameState.GameOver);
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        resumeButton.SetActive(false);
        title.text = text;
    }

    private void OnSceneLoaded()
    {
        // Restauramos el tiempo solo si venimos de un cambio de escena, no si el juego está pausado por GameOver
        if (Time.timeScale == 0f && GameStateManager.Instance.CurrentGameState != GameState.Paused)
        {
            Time.timeScale = 1f;
        }
    }
}
 */

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIPauseManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject resumeButton;
    [SerializeField] TMP_Text title;

    void Start()
    {
        GameManager.OnGameEnded += ShowEndScreen;
        GameSceneManager.OnSceneFullyLoaded += OnSceneLoaded;
        GameStateManager.Instance.Paused.OnPausedGame += PauseGame;
        GameStateManager.Instance.Paused.OnResumedGame += ResumeGame;
    }


    private void OnDisable()
    {
        GameManager.OnGameEnded -= ShowEndScreen;
        GameSceneManager.OnSceneFullyLoaded -= OnSceneLoaded;
        GameStateManager.Instance.Paused.OnPausedGame -= PauseGame;
        GameStateManager.Instance.Paused.OnResumedGame -= ResumeGame;
    }

    public void PauseGame()
    {
        Debug.Log("Pause Game");
        //GameStateManager.Instance.EnterPause();
        pauseMenu.SetActive(true);
        resumeButton.SetActive(true);
        title.text = "PAUSED";
    }

    public void ResumeGame()
    {
        Debug.Log("Resume");
        GameStateManager.Instance.ExitPause();
        pauseMenu.SetActive(false);
    }

    public void RestartGame()
    {
        ResumeGame();
        GameStateManager.Instance.ExitPause();
        
        GameManager.Instance.SetMaxHealth();
        StartCoroutine(GameSceneManager.Instance.LoadSceneWithTransitionRoutine(SceneManager.GetActiveScene().name));
    }

    public void ReturnToMenu()
    {
        ResumeGame();
        StartCoroutine(GameSceneManager.Instance.LoadSceneWithTransitionRoutine("MainMenu"));
    }

    public void ShowEndScreen(bool won, string text)
    {
        pauseMenu.SetActive(true);
        resumeButton.SetActive(false);
        title.text = text;
    }

    private void OnSceneLoaded()
    {
        // Restauramos el tiempo solo si no estamos en pausa ni en GameOver
        if (Time.timeScale == 0f &&
            GameStateManager.Instance.StateMachine.CurrentState != GameStateManager.Instance.Paused &&
            GameStateManager.Instance.StateMachine.CurrentState != GameStateManager.Instance.GameOver)
        {
            Time.timeScale = 1f;
        }
    }
}