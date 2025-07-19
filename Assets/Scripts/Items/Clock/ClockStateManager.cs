using UnityEngine;

public class ClockStateManager : MonoBehaviour
{
    public static ClockStateManager Instance;

    private int savedHour;
    private int savedMinute;
    private bool restoreRequested = false;

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

    public void SaveClockTime()
    {
        if (GameClock.Instance != null)
        {
            savedHour = GameClock.Instance.hour;
            savedMinute = GameClock.Instance.minute;
        }
    }

    public void RequestRestore()
    {
        restoreRequested = true;
    }

    private void Start()
    {
        if (restoreRequested && GameClock.Instance != null)
        {
            GameClock.Instance.SetTime(savedHour, savedMinute);
            restoreRequested = false;
        }
    }
}
