using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    public GameClock clock;

    public GameObject[] dayObjects;
    public GameObject[] nightObjects;

    public int dayStartHour = 6;
    public int nightStartHour = 18;

    private bool isDay;

    void Start()
    {
        if (clock.hour >= dayStartHour && clock.hour < nightStartHour)
        {
            SetDay(true);
            isDay = true;
        }
        else
        {
            SetDay(false);
            isDay = false;
        }
    }

    void Update()
    {
        if (clock.hour >= dayStartHour && clock.hour < nightStartHour)
        {
            if (!isDay)
            {
                SetDay(true);
                isDay = true;
            }
        }
        else
        {
            if (isDay)
            {
                SetDay(false);
                isDay = false;
            }
        }
    }

    void SetDay(bool active)
    {
        foreach (GameObject obj in dayObjects)
            obj.SetActive(active);
        foreach (GameObject obj in nightObjects)
            obj.SetActive(!active);
    }
}