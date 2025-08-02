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

        if (LevelManagerTemp.Instance != null && LevelManagerTemp.Instance.enabled)
        {
            Debug.Log("LevelManagerTemp está activo.");
        }
        else
        {
            Debug.LogWarning("No se encontró LevelManagerTemp en la escena o no está activo.");
        }
    }
}
