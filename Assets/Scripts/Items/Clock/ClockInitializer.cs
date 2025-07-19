using UnityEngine;

public class ClockInitializer : MonoBehaviour
{
    void Awake()
    {
        if (GameClock.Instance == null)
        {
            GameObject clockPrefab = Resources.Load<GameObject>("Prefabs/GameClock");
            Instantiate(clockPrefab);
        }

        if (ClockMemory.Instance == null)
        {
            GameObject memory = new GameObject("ClockMemory");
            memory.AddComponent<ClockMemory>();
        }
    }
}
