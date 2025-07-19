using UnityEngine;
using UnityEngine.SceneManagement;

public class ClockMemory : MonoBehaviour
{
    public static ClockMemory Instance;

    private int savedHour;
    private int savedMinute;
    private string savedScene;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveTime()
    {
        if (GameClock.Instance != null)
        {
            GameClock.Instance.GetTime(out savedHour, out savedMinute);
            savedScene = SceneManager.GetActiveScene().name;
            Debug.Log($"[ClockMemory] Tiempo guardado: {savedHour:00}:{savedMinute:00}");
        }
    }

    public void RestoreTime()
    {
        if (GameClock.Instance != null)
        {
            GameClock.Instance.SetTime(savedHour, savedMinute);
            Debug.Log($"[ClockMemory] Tiempo restaurado: {savedHour:00}:{savedMinute:00}");
        }
    }

    public string GetSavedScene()
    {
        return savedScene;
    }
}
