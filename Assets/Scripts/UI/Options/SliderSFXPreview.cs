using UnityEngine;
using UnityEngine.EventSystems;

public class SliderSFXPreview : MonoBehaviour, IPointerUpHandler
{
    [SerializeField] AudioSource sfxExample;

    public void OnPointerUp(PointerEventData eventData)
    {
        if (sfxExample != null)
        {
            sfxExample.Play();
        }
    }
}