using UnityEngine;

public class DetectObjective : MonoBehaviour
{
    [SerializeField] GameObject[] objetsToActivate;
    [SerializeField] GameObject[] objetsToDeactivate;
    [SerializeField] PuzzleGridManager puzzle;
    [SerializeField] bool killEnemies;

    void Start()
    {
        if (puzzle != null)
            puzzle.OnCompleted += PuzzleCompleted;
    }

    void Update()
    {
        if(killEnemies)
            CheckEnemies();
    }

    void CheckEnemies()
    {
        GameObject[] remainingEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (remainingEnemies.Length == 0)
        {
            EnemiesEliminated();
        }
    }

    void OnDestroy()
    {
        if (puzzle != null)
            puzzle.OnCompleted -= PuzzleCompleted;
    }

    void PuzzleCompleted()
    {
        Invoke("ActivateDoor", 1f);
        if (objetsToDeactivate.Length > 0)
            Invoke("DeactivateObjects", 1f);
    }

    void ActivateDoor()
    {
        foreach(GameObject obj in objetsToActivate)
            obj.SetActive(true);
    }

    void DeactivateObjects()
    {
        foreach(GameObject obj in objetsToDeactivate)
            obj.SetActive(false);
    }

    void EnemiesEliminated()
    {
        Invoke("ActivateDoor", 2f);
        if (objetsToDeactivate.Length > 0)
            Invoke("DeactivateObjects", 2f);
    }
    public void ActivateDoorDirectly()
    {
        ActivateDoor();
    }

}