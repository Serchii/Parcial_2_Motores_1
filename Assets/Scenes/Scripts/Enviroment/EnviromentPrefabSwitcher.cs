using UnityEngine;

public class EnvironmentPrefabSwitcher : MonoBehaviour
{
    public GameClock clock;

    public GameObject dayPrefab;
    public GameObject nightPrefab;

    private GameObject currentInstance;

    public int dayStartHour = 6;
    public int nightStartHour = 18;

    void Update()
    {
        if (clock.hour >= dayStartHour && clock.hour < nightStartHour)
        {
            if (currentInstance == null || currentInstance.name != dayPrefab.name + "(Clone)")
            {
                SwitchTo(dayPrefab);
            }
        }
        else
        {
            if (currentInstance == null || currentInstance.name != nightPrefab.name + "(Clone)")
            {
                SwitchTo(nightPrefab);
            }
        }
    }

    void SwitchTo(GameObject prefab)
    {
        if (currentInstance != null)
            Destroy(currentInstance);

        currentInstance = Instantiate(prefab, transform.position, Quaternion.identity);
    }
}