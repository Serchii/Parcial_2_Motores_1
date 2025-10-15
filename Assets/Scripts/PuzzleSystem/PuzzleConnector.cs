using UnityEngine;

public class PuzzleConnector : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private BossHealth bossHealth;
    [SerializeField] private PuzzleTrigger[] puzzleTriggers;

    private int currentPuzzleIndex = 0;

    private void Start()
    {
        bossHealth.OnBossStunned.AddListener(OnBossStunned);
        bossHealth.OnPuzzleResult.AddListener(OnPuzzleResult);
    }

    private void OnBossStunned()
    {
        if (currentPuzzleIndex < puzzleTriggers.Length)
        {
            PuzzleTrigger puzzle = puzzleTriggers[currentPuzzleIndex];
            if (puzzle != null)
            {
                puzzle.SetInteractuable(true);
                puzzle.gameObject.SetActive(true);
                Debug.Log("Puzzle " + (currentPuzzleIndex + 1) + " activado.");
            }
        }
    }

    private void OnPuzzleResult(bool solved)
    {
        bossHealth.ResumeFightAfterPuzzle(solved);

        if (currentPuzzleIndex < puzzleTriggers.Length)
        {
            PuzzleTrigger puzzle = puzzleTriggers[currentPuzzleIndex];
            if (puzzle != null)
            {
                puzzle.SetInteractuable(false);
                puzzle.gameObject.SetActive(false);
            }
        }

        currentPuzzleIndex++;
    }
}
