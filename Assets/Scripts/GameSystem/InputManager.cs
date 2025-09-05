using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    [SerializeField] bool gameplay = true;
    public float Horizontal;

    public event Action OnJumpPressed;
    public event Action OnAttackPressed;
    public event Action OnDashPressed;
    public event Action OnDownPressed;
    public event Action OnInteractPressed;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    void OnDisable()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }



    // Update is called once per frame
    void Update()
    {
        if (gameplay)
        {
            Horizontal = Input.GetAxisRaw("Horizontal");

            if (Input.GetButtonDown("Jump"))
            {
                OnJumpPressed?.Invoke();
            }

            if (Input.GetButtonDown("Attack"))
            {
                OnAttackPressed?.Invoke();
            }

            if (Input.GetButtonDown("Dash"))
            {
                OnDashPressed?.Invoke();
            }

            if (Input.GetButtonDown("Down"))
            {
                OnDownPressed?.Invoke();
            }
        }
        else
            Horizontal = 0;

        if (Input.GetButtonDown("Interact"))
        {
            OnInteractPressed?.Invoke();
        }
    }

    private void OnGameStateChanged(GameState newGameState)
    {
        gameplay = newGameState == GameState.Gameplay;
    }
}
