using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UITransitionManager : MonoBehaviour
{
    [SerializeField] float transitionTime = 1f;
    [SerializeField] Animator transitionAnimator;

    [SerializeField] Slider loadbar;
    [SerializeField] GameObject loadPanel;
    [SerializeField] GameObject transitionBackground;

    void Start()
    {
        if (transitionAnimator == null)
            transitionAnimator = GetComponentInChildren<Animator>();
    }

    public IEnumerator PlayTransition()
    {
        if (transitionAnimator != null)
        {
            EnableImage();
            transitionAnimator.SetTrigger("StartTransition");
            yield return new WaitForSeconds(transitionTime);
        }
    }

    public void ShowLoadPanel()
    {
        if (loadPanel != null)
            loadPanel.SetActive(true);
    }

    public void EnableImage()
    {
        transitionBackground.SetActive(true);
    }

    public void DisableImage()
    {
        transitionBackground.SetActive(false);
    }

    public void UpdateLoadbar(float progress)
    {
        if (loadbar != null)
            loadbar.value = progress;
    }

}
