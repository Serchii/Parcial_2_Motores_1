using UnityEngine;
using System;

public class GameClock : MonoBehaviour
{
    public static GameClock Instance;

    [Header("Tiempo actual")]
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

    public void SetTime(int h, int m)
    {
        hour = Mathf.Clamp(h, 0, 23);
        minute = Mathf.Clamp(m, 0, 59);
        _timer = 0f;
    }

    public void GetTime(out int h, out int m)
    {
        h = hour;
        m = minute;
    }

    public int GetHour()
    {
        return hour;
    }

    public int GetMinute()
    {
        return minute;
    }

    public void ResetClock()
    {
        hour = 0;
        minute = 0;
        _timer = 0f;
    }
}
