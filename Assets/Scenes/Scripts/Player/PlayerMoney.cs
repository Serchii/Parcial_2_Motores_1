using UnityEngine;

public class PlayerMoney : MonoBehaviour
{
    public static PlayerMoney Instance;

    [SerializeField] private int _money;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadMoney();
    }

    public int GetMoney()
    {
        return _money;
    }

    public void AddMoney(int amount)
    {
        _money += amount;
        SaveMoney();
    }

    public bool SpendMoney(int amount)
    {
        if (_money >= amount)
        {
            _money -= amount;
            SaveMoney();
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SaveMoney()
    {
        PlayerPrefs.SetInt("PlayerMoney", _money);
    }

    private void LoadMoney()
    {
        _money = PlayerPrefs.GetInt("PlayerMoney", 0);
    }
}
