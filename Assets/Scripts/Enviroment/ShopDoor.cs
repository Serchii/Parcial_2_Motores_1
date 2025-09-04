using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopDoor : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    // Start is called before the first frame update
    void OnEnable()
    {
        audioSource.Play();
        Invoke("DisableObject",2f);
    }

    // Update is called once per frame
    void DisableObject()
    {
        gameObject.SetActive(false);
    }
}
