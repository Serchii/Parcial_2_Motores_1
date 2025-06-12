using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public ClockUI clockUI;

    void Awake()
    {
        Instance = this;
    }

    public void ShowClockUI(bool show)
    {
        clockUI.ShowClock(show);
    }
}