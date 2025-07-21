using TMPro;
using UnityEngine;

public class ClockUI : MonoBehaviour
{
    [SerializeField] private GameObject _clockPanel;
    [SerializeField] private TMP_Text _clockText;

    private void Start()
    {
        if (PlayerInventory.Instance != null && _clockPanel != null)
        {
            bool tieneReloj = PlayerInventory.Instance.HasItem(ItemID.Watch);
            _clockPanel.SetActive(tieneReloj);
        }
    }

    private void Update()
    {
        if (_clockPanel != null && _clockPanel.activeSelf && GameClock.Instance != null)
        {
            _clockText.text = $"{GameClock.Instance.hour:00}:{GameClock.Instance.minute:00}";
        }
    }

    public void ShowClock(bool show)
    {
        if (_clockPanel != null && PlayerInventory.Instance.HasItem(ItemID.Watch))
            _clockPanel.SetActive(show);
    }
}
