using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    private static GameStateManager instance;
    public static GameStateManager Instance => instance;

    public GameState CurrentGameState
    {
        get;
        private set;
    }

    public delegate void GameStateChangeHandler(GameState newGameState);
    public event GameStateChangeHandler OnGameStateChanged;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private GameStateManager()
    {

    }

    public void SetState(GameState newGameState)
    {
        if (newGameState == CurrentGameState)
            return;

        CurrentGameState = newGameState;
        OnGameStateChanged?.Invoke(newGameState);
    }
}
