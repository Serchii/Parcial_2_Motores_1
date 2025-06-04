using UnityEngine;

public class UIFollowPlayer : MonoBehaviour
{
    [SerializeField] Vector3 offset;

    private Transform target;

    public void SetTarget(Transform player)
    {
        target = player;
    }

    void Update()
    {
        if (target != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position + offset);
            transform.position = screenPos;
        }
    }
}
