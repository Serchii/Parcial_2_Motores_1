using UnityEngine;

public class UIFollowPlayer : MonoBehaviour
{
    [SerializeField] Vector3 offset;

    private Transform target;
    private Camera cam;

    public void SetTarget(Transform player)
    {
        target = player;
    }

    void Start()
    {
        cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("No se encontró ninguna cámara con el tag 'MainCamera'");
        }
    }

    void Update()
    {
        if (target != null && cam != null)
        {
            Vector3 screenPos = cam.WorldToScreenPoint(target.position + offset);
            transform.position = screenPos;
        }
    }
}