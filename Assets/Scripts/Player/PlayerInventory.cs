using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    public bool hasWatch;

    void Awake()
    {
        Instance = this;

        // Cargar el estado desde PlayerPrefs
        hasWatch = PlayerPrefs.GetInt("HasWatch", 0) == 1;

        if (hasWatch)
            UIManager.Instance.ShowClockUI(true);
    }

    public void BuyWatch()
    {
        hasWatch = true;
        PlayerPrefs.SetInt("HasWatch", 1);
        UIManager.Instance.ShowClockUI(true);
    }
}