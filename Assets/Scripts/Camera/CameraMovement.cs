using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform playerPos;
    [SerializeField] Vector3 cameraPos;
    [SerializeField] float leftLimit;
    [SerializeField] float rightLimit;
    [SerializeField] float posX;
    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (playerPos == null) return;
        if (playerPos.position.x < leftLimit)
        {
            posX = leftLimit;
        }
        else if (playerPos.position.x > rightLimit)
        {
            posX = rightLimit;
        }
        else
        {
            posX = playerPos.position.x;
        }
        Vector3 targetPos = new Vector3(posX, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, 5f * Time.deltaTime);

        
    }
}
