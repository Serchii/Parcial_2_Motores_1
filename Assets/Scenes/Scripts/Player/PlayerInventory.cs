using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    public bool hasWatch;
    public bool hasExpertCertificate;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadData();
    }

    public void BuyWatch()
    {
        hasWatch = true;
        UIManager.Instance.ShowClockUI(true);
        SaveData();
    }

    public void BuyExpertCertificate()
    {
        hasExpertCertificate = true;
        SaveData();
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt("HasWatch", hasWatch ? 1 : 0);
        PlayerPrefs.SetInt("HasExpertCertificate", hasExpertCertificate ? 1 : 0);
    }

    private void LoadData()
    {
        hasWatch = PlayerPrefs.GetInt("HasWatch", 0) == 1;
        hasExpertCertificate = PlayerPrefs.GetInt("HasExpertCertificate", 0) == 1;

        if (hasWatch)
            UIManager.Instance.ShowClockUI(true);
    }
}
