using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    public void ChangeScene()
    {
        GameSceneManager.Instance.LoadSceneWithTransition("MainMenu");
    }
}
