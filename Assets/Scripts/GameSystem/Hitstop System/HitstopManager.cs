using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitstopManager : MonoBehaviour
{
    public static HitstopManager Instance;

    private bool isHitstopping;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void DoHitstop(float duration)
    {
        if (!isHitstopping)
            StartCoroutine(HitstopCoroutine(duration));
    }

    private IEnumerator HitstopCoroutine(float duration)
    {
        isHitstopping = true;
        float originalTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = originalTimeScale;
        isHitstopping = false;
    }
}
