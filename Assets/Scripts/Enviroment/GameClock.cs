using UnityEngine;

public class GameClock : MonoBehaviour
{
    public static GameClock Instance;

    public int hour;
    public int minute;

    [SerializeField] private float _timeSpeed = 60f;
    private float _timer;

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

    private void Update()
    {
        _timer += Time.deltaTime * _timeSpeed;

        if (_timer >= 60f)
        {
            minute++;
            _timer = 0f;

            if (minute >= 60)
            {
                hour++;
                minute = 0;
                if (hour >= 24)
                    hour = 0;
            }
        }
    }

    public void ResetClock()
    {
        hour = 0;
        minute = 0;
        _timer = 0f;
    }
}
