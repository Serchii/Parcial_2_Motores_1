using UnityEngine;

public class ClockInitializer : MonoBehaviour
{
    void Awake()
    {
        if (GameClock.Instance == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/GameClock");
            Instantiate(prefab);
            Debug.Log("GameClock instanciado automáticamente.");
        }
    }
}

