using System;
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
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // Nos enganchamos al evento de la FSM
        GameStateManager.Instance.StateMachine.OnStateChanged += OnStateChanged;
    }

    void OnDisable()
    {
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.StateMachine.OnStateChanged -= OnStateChanged;
    }

    void Update()
    {
        if (gameplay)
        {
            Horizontal = Input.GetAxisRaw("Horizontal");

            if (Input.GetButtonDown("Jump")) OnJumpPressed?.Invoke();
            if (Input.GetButtonDown("Attack")) OnAttackPressed?.Invoke();
            if (Input.GetButtonDown("Dash")) OnDashPressed?.Invoke();
            if (Input.GetButtonDown("Down")) OnDownPressed?.Invoke();
        }
        else
        {
            Horizontal = 0;
        }

        if (Input.GetButtonDown("Interact"))
        {
            OnInteractPressed?.Invoke();
        }
    }

    private void OnStateChanged(IState newState)
    {
        // Solo dejamos input habilitado si el estado es GameplayState
        gameplay = newState is GameplayState;
    }
}