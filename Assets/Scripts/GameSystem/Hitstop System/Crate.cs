using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    SpriteRenderer rend;
    Shader hitShader;
    public GameObject breakPrefab;
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        hitShader = Shader.Find("GUI/TextShader");
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            rend.material.shader = hitShader;
            rend.material.color = Color.white;

            Instantiate(breakPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}