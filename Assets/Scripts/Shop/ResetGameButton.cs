using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResetGameButton : MonoBehaviour
{
    [SerializeField] private Button resetButton;

    private void Start()
    {
        resetButton.onClick.AddListener(ResetGame);
    }

    private void ResetGame()
    {
        GameManager.Instance.ResetGameData();
    }
}
