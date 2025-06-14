using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public int Money = 1000;
    public HashSet<string> Inventory = new HashSet<string>();
}