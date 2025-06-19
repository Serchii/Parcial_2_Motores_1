using UnityEngine;

public class DetectObjective : MonoBehaviour
{
    [SerializeField] GameObject nextLevel;
    [SerializeField] PuzzleGridManager puzzle;

    void Start()
    {
        if (puzzle != null)
            puzzle.OnCompleted += ActivateDoor;
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
}