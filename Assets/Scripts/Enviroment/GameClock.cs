using UnityEngine;

public class GameClock : MonoBehaviour
{
    public int hour;
    public int minute;

    public float timeSpeed = 60f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime * timeSpeed;

        if (timer >= 60f)
        {
            minute++;
            timer = 0f;

            if (minute >= 60)
            {
                hour++;
                minute = 0;
                if (hour >= 24)
                    hour = 0;
            }
        }
    }
}
