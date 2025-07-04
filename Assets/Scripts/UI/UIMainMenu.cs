using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] GameObject[] panels;
    [SerializeField] string nextLevel;

    public void Credits()
    {
        SetPanel(2);
    }

    public void Options()
    {
        SetPanel(1);
    }

    public void Story()
    {
        GameSceneManager.Instance.LoadSceneWithTransition("IntroText");
        GameManager.Instance.SetMaxHealth();
        PlayerInventory.Instance.ResetInventory(); //Aca estaba el problema
    }

    public void NextLevel()
    {
        GameSceneManager.Instance.LoadSceneWithTransition(nextLevel);
    }


    public void BackToMenu()
    {
        SetPanel(0);
    }

    void SetPanel(int indexPanel)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            if (i == indexPanel)
            {
                panels[i].SetActive(true);
                continue;
            }

            panels[i].SetActive(false);
        }
    }

    public void StartGame()
    {
        GameSceneManager.Instance.LoadSceneWithTransition("Classroom");
    }
}