using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Player")]
    private Transform _playerPos;

    [Header("Camera Settings")]
    [SerializeField] private Vector3 _offset = new Vector3(0f, 0f, -10f);
    [SerializeField] private float _smoothSpeed = 5f;

    [Header("Horizontal Limits")]
    [SerializeField] private float _leftLimit = -10f;
    [SerializeField] private float _rightLimit = 10f;

    private void Start()
    {
        if (_playerPos == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                _playerPos = playerObj.transform;
            }
            else
            {
                Debug.LogError("No se encontró ningún objeto con el tag 'Player'.");
            }
        }
    }

    private void LateUpdate()
    {
        if (_playerPos == null) return;

        float clampedX = Mathf.Clamp(_playerPos.position.x, _leftLimit, _rightLimit);
        Vector3 targetPosition = new Vector3(clampedX, _playerPos.position.y, 0f) + _offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, _smoothSpeed * Time.deltaTime);
    }
}