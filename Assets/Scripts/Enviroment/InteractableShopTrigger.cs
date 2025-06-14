using UnityEngine;

public class InteractableShopTrigger : MonoBehaviour
{
    private bool _playerInRange = false;

    void Update()
    {
        if (_playerInRange && Input.GetButtonDown("Interact"))
        {
            ShopUI.Instance.OpenShop();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = true;

            Debug.Log("Presiona F para abrir la tienda.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = false;
        }
    }
}