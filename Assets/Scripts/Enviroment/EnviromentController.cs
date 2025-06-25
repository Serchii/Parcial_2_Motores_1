using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    [Header("Objetos de ambiente")]
    [SerializeField] private GameObject[] dayObjects;
    [SerializeField] private GameObject[] nightObjects;

    [Header("Horario")]
    [SerializeField] private int dayStartHour = 6;
    [SerializeField] private int nightStartHour = 18;

    private bool isDay = true;

    void Start()
    {
        UpdateEnvironment();
    }

    void Update()
    {
        if (GameClock.Instance == null)
            return;

        int hour = GameClock.Instance.hour;

        if (hour >= dayStartHour && hour < nightStartHour)
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

    void UpdateEnvironment()
    {
        if (GameClock.Instance == null)
            return;

        int hour = GameClock.Instance.hour;
        bool shouldBeDay = hour >= dayStartHour && hour < nightStartHour;

        SetDay(shouldBeDay);
        isDay = shouldBeDay;
    }
}
