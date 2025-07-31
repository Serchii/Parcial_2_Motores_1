using UnityEngine;

public class TimeSetter : MonoBehaviour
{
    [SerializeField] private int hour;
    [SerializeField] private int minute;

    void Start()
    {
        GameClock clock = FindObjectOfType<GameClock>();
        if (clock != null)
        {
            clock.SetTime(hour, minute);
            Debug.Log($"Hora seteada a: {hour:00}:{minute:00}");
        }

        LevelManager levelManager = FindObjectOfType<LevelManager>();
        if (levelManager != null && levelManager.enabled)
        {
            levelManager.StartLevelFromSession();
        }
        else
        {
            Debug.LogWarning("No se encontró el LevelManager o está deshabilitado.");
        }
    }
}
