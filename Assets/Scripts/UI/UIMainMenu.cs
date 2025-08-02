using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] GameObject[] panels;
    [SerializeField] string nextLevel;
    [SerializeField] Animator animator;
    [SerializeField] bool SetMaxHealth = false;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public void Credits()
    {
        SetPanel(2);
    }

    public void Options()
    {
        SetPanel(1);
    }

    public void BackToMenu()
    {
        SetPanel(0);
    }

    public void TransitionOptionsToMenu()
    {
        animator.SetTrigger("OptionsToMenu");
    }

    public void TransitionCreditsToMenu()
    {
        animator.SetTrigger("CreditsToMenu");
    }

    public void TransitionOptions()
    {
        animator.SetTrigger("MenuToOptions");
    }

    public void TransitionCredits()
    {
        animator.SetTrigger("MenuToCredits");
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
        if (SetMaxHealth)
            GameManager.Instance.SetMaxHealth();
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