using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] GameObject [] panels;

    public void Credits()
    {
        SetPanel(3);
    }

    public void Options()
    {
        SetPanel(2);
    }

    public void HowToPlay()
    {
        SetPanel(1);
    }

    public void BackToMenu()
    {
        SetPanel(0);
    }

    void SetPanel(int indexPanel)
    {
        for(int i = 0; i < panels.Length; i++)
        {
            if(i == indexPanel)
            {
                panels[i].SetActive(true);
                continue;
            }

            panels[i].SetActive(false);
        }
    }
    
    public void StartGame()
    {
        GameSceneManager.Instance.LoadSceneWithTransition("Level_1");
    }
}
