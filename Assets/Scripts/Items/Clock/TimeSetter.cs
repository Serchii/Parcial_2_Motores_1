using UnityEngine;

public class TimeSetter : MonoBehaviour
{
    [Header("Hora deseada")]
    [Range(0, 23)] public int hour = 6;
    [Range(0, 59)] public int minute = 0;

    [Header("Control de inicio")]
    public bool iniciarManualmente = true;
    public float delayAntesDeIniciar = 0.1f;

    private void Start()
    {
        if (GameClock.Instance != null)
        {
            GameClock.Instance.SetTime(hour, minute);
            Debug.Log($"? Hora seteada manualmente a {hour:00}:{minute:00}");
        }
        else
        {
            Debug.LogWarning("GameClock.Instance no está disponible al inicio.");
        }

        if (iniciarManualmente)
        {
            Invoke(nameof(ArrancarNivel), delayAntesDeIniciar);
        }
    }

    void ArrancarNivel()
    {
        LevelManager manager = FindObjectOfType<LevelManager>();
        if (manager != null)
        {
            manager.StartLevelManualmente();
            Debug.Log("? Nivel iniciado manualmente por TimeSetter.");
        }
        else
        {
            Debug.LogWarning("? No se encontró el LevelManager en la escena.");
        }
    }
}