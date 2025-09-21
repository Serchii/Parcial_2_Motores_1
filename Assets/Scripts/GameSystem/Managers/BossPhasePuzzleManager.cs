using System;
using UnityEngine;
using UnityEngine.Events;


public class BossPhasePuzzleManager : MonoBehaviour
{
    [Header("Referencias")]
    public BossHealth boss;
    [Tooltip("Asignar un puzzle por cada fase. Debe haber 3 elementos.")]
    public GameObject[] puzzleUIObjects;
    [Tooltip("Tiempo máximo para resolver cada puzzle (segundos)")]
    public float puzzleTimeLimit = 12f;

    [Header("Eventos")]
    public UnityEvent OnPuzzleStart;
    public UnityEvent OnPuzzleSuccess;
    public UnityEvent OnPuzzleFail;

    private int activePhase = -1;
    private float timer = 0f;
    private bool puzzleActive = false;

    private PuzzleGridManager activeGridManager;

    private void Start()
    {
        if (boss == null)
            boss = FindObjectOfType<BossHealth>();

        boss.OnBossStunned.AddListener(OnBossStunned);
    }

    private void Update()
    {
        if (!puzzleActive) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            FailPuzzle();
        }
    }

    private void OnBossStunned()
    {
        activePhase = boss.GetCurrentPhaseIndex();

        if (activePhase < 0 || activePhase >= puzzleUIObjects.Length)
        {
            Debug.LogWarning("BossPhasePuzzleManager: no hay puzzle UI asignado para la fase " + activePhase);
            boss.ResumeFightAfterPuzzle(false);
            return;
        }

        StartPuzzleForPhase(activePhase);
    }

    private void StartPuzzleForPhase(int phaseIndex)
    {
        GameObject puzzleGO = puzzleUIObjects[phaseIndex];
        if (puzzleGO == null)
        {
            Debug.LogWarning("Puzzle GO nulo para fase " + phaseIndex);
            boss.ResumeFightAfterPuzzle(false);
            return;
        }

        if (GameStateManager.Instance != null)
            GameStateManager.Instance.EnterPuzzle();

        puzzleGO.SetActive(true);
        timer = puzzleTimeLimit;
        puzzleActive = true;
        OnPuzzleStart?.Invoke();

        activeGridManager = puzzleGO.GetComponentInChildren<PuzzleGridManager>();
        if (activeGridManager != null)
        {
            activeGridManager.OnCompleted += HandlePuzzleSolved;
        }
        else
        {
            Debug.Log("BossPhasePuzzleManager: no se encontró PuzzleGridManager en el GO. Esperando resolución manual.");
        }

        Debug.Log($"Puzzle fase {phaseIndex} iniciado. Tiempo: {puzzleTimeLimit}s");
    }

    private void HandlePuzzleSolved()
    {
        if (!puzzleActive) return;
        SuccessPuzzle();
    }

    private void SuccessPuzzle()
    {
        puzzleActive = false;
        CleanupActivePuzzle();
        OnPuzzleSuccess?.Invoke();

        boss.ResumeFightAfterPuzzle(true);

        if (GameStateManager.Instance != null)
            GameStateManager.Instance.ExitPuzzle();

        Debug.Log("Puzzle resuelto a tiempo.");
    }

    private void FailPuzzle()
    {
        puzzleActive = false;
        CleanupActivePuzzle();
        OnPuzzleFail?.Invoke();

        boss.ResumeFightAfterPuzzle(false);

        if (GameStateManager.Instance != null)
            GameStateManager.Instance.ExitPuzzle();

        Debug.Log("Puzzle fallado o tiempo vencido.");
    }

    private void CleanupActivePuzzle()
    {
        if (activePhase >= 0 && activePhase < puzzleUIObjects.Length)
        {
            GameObject puzzleGO = puzzleUIObjects[activePhase];
            if (puzzleGO != null)
                puzzleGO.SetActive(false);
        }

        if (activeGridManager != null)
        {
            activeGridManager.OnCompleted -= HandlePuzzleSolved;
            activeGridManager = null;
        }

        activePhase = -1;
    }

    public void ResolvePuzzleManually(bool solved)
    {
        if (!puzzleActive) return;
        if (solved) SuccessPuzzle();
        else FailPuzzle();
    }
}
