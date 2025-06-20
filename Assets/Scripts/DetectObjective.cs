using UnityEngine;

public class DetectObjective : MonoBehaviour
{
    [SerializeField] GameObject nextLevel;
    [SerializeField] PuzzleGridManager puzzle;
    [SerializeField] bool killEnemies;

    void Start()
    {
        if (puzzle != null)
            puzzle.OnCompleted += ActivateDoor;
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
            puzzle.OnCompleted -= ActivateDoor;
    }

    void ActivateDoor()
    {
        nextLevel.SetActive(true);
    }

    void EnemiesEliminated()
    {
        Invoke("ActivateDoor", 2f);
    }
}