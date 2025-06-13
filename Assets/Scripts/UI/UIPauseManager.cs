using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIPauseManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject resumeButton;
    [SerializeField] TMP_Text title;

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
            if (!isPaused)
                PauseGame();
            else
                ResumeGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        title.text = "PAUSED";
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    public void RestartGame()
    {
        ResumeGame();
        StartCoroutine(GameSceneManager.Instance.LoadSceneWithTransitionRoutine(SceneManager.GetActiveScene().name));
    }

    public void ReturnToMenu()
    {
        ResumeGame();
        StartCoroutine(GameSceneManager.Instance.LoadSceneWithTransitionRoutine("MainMenu"));
    }

    public void ShowEndScreen(bool won, string text)
    {
        isPaused = true;
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        resumeButton.SetActive(false);
        title.text = text;
    }

    private void OnSceneLoaded()
    {
        // Restauramos el tiempo solo si venimos de un cambio de escena, no si el juego está pausado por GameOver
        if (Time.timeScale == 0f && !isPaused)
        {
            Time.timeScale = 1f;
        }
    }
}
