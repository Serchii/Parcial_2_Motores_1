using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClockUI : MonoBehaviour
{
    public GameClock clock;
    public GameObject clockPanel;
    public TMP_Text clockText;

    void Update()
    {
        if (clockPanel.activeSelf)
        {
            clockText.text = $"{clock.hour:00}:{clock.minute:00}";
        }
    }

    public void ShowClock(bool show)
    {
        clockPanel.SetActive(show);
    }
}