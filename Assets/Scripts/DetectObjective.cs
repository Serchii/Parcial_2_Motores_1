using UnityEngine;

public class DetectObjective : MonoBehaviour
{
    [SerializeField] GameObject[] objetsToActivate;
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
    }

    void ActivateDoor()
    {
        foreach(GameObject obj in objetsToActivate)
            obj.SetActive(true);
    }

    void EnemiesEliminated()
    {
        Invoke("ActivateDoor", 2f);
    }
    public void ActivateDoorDirectly()
    {
        ActivateDoor();
    }

}