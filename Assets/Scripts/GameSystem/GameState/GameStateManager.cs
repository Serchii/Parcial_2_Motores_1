using System;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }
    public StateMachine StateMachine { get; private set; }

    private bool gameEnded = false;
    private bool gamePaused = false;
    private bool dialogRequested = false;
    private bool puzzleRequested = false;
    private bool shopRequested = false;

    // Estados del juego
    public GameplayState Gameplay { get; private set; }
    public PausedState Paused { get; private set; }
    public DialogState Dialog { get; private set; }
    public PuzzleState Puzzle { get; private set; }
    public ShopState Shop { get; private set; }
    public GameOverState GameOver { get; private set; }

    void At(IState from, IState to, IPredicate condition) => StateMachine.AddTransition(from, to, condition);
    void Any(IState to, IPredicate condition) => StateMachine.AddAnyTransition(to, condition);

    public void EnterPuzzle() => puzzleRequested = true;
    public void ExitPuzzle() => puzzleRequested = false;
    public void EnterPause() => gamePaused = true;
    public void ExitPause() => gamePaused = false;
    public void EnterDialog() => dialogRequested = true;
    public void ExitDialog() => dialogRequested = false;
    public void EnterShop() => shopRequested = true;
    public void ExitShop() => shopRequested = false;

    void Awake()
    {
        if (Instance == null) Instance = this;

        // Inicializamos máquina de estados
        StateMachine = new StateMachine();

        // Declaramos los estados
        Gameplay = new GameplayState();
        Paused = new PausedState();
        Dialog = new DialogState();
        Puzzle = new PuzzleState();
        Shop = new ShopState();
        GameOver = new GameOverState();

        // Transiciones entre Gameplay y Paused
        At(Gameplay, Paused, new FuncPredicate(() => gamePaused));
        At(Paused, Gameplay, new FuncPredicate(() => !gamePaused));

        // Dialog
        At(Gameplay, Dialog, new FuncPredicate(() => dialogRequested));
        At(Dialog, Gameplay, new FuncPredicate(() => !dialogRequested));

        // Puzzle
        At(Gameplay, Puzzle, new FuncPredicate(() => puzzleRequested));
        At(Puzzle, Gameplay, new FuncPredicate(() => !puzzleRequested));

        // Shop
        At(Gameplay, Shop, new FuncPredicate(() => shopRequested));
        At(Shop, Gameplay, new FuncPredicate(() => !shopRequested));

        // GameOver
        Any(GameOver, new FuncPredicate(() => gameEnded));

        // Estado inicial
        StateMachine.SetState(Gameplay);
    }

    void Update() => StateMachine.Update();
    void FixedUpdate() => StateMachine.FixedUpdate();

    public void NotifyGameOver()
    {
        gameEnded = true;
    }    


}

public class GameplayState : State
{
    public override void OnEnter()
    {
        Time.timeScale = 1f;
        Debug.Log("Gameplay ON");
    }

    public override void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            GameStateManager.Instance.EnterPause();
        }
    }
}

public class PausedState : State
{
    public event Action OnPausedGame;
    public event Action OnResumedGame;
    public override void OnEnter()
    {
        Time.timeScale = 0f;
        Debug.Log("Juego Pausado");
        OnPausedGame?.Invoke();
    }

    public override void OnExit()
    {
        Time.timeScale = 1f;
        OnResumedGame?.Invoke();
    }

    public override void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            GameStateManager.Instance.ExitPause();
        }
    }
}

public class DialogState : State
{
    public override void OnEnter()
    {
        Debug.Log("Entrando a Diálogo");
    }

    public override void OnExit()
    {
        
    }
}

public class PuzzleState : State
{
    public override void OnEnter()
    {
        Debug.Log("Entrando a Puzzle");
    }

    public override void OnExit()
    {

    }
}

public class ShopState : State
{
    public override void OnEnter()
    {
        Debug.Log("Entrando a Tienda");
    }

    public override void OnExit()
    {

    }
}

public class GameOverState : State
{
    public override void OnEnter()
    {
        Debug.Log("Game Over");
        Time.timeScale = 0f;
    }
}
