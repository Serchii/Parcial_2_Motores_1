using TMPro;
using UnityEngine;

public class ClockUI : MonoBehaviour
{
    [SerializeField] private GameObject _clockPanel;
    [SerializeField] private TMP_Text _clockText;

    private void Update()
    {
        if (_clockPanel.activeSelf && GameClock.Instance != null)
        {
            _clockText.text = $"{GameClock.Instance.hour:00}:{GameClock.Instance.minute:00}";
        }
    }

    public void ShowClock(bool show)
    {
        if (_clockPanel != null)
            _clockPanel.SetActive(show);
    }
}