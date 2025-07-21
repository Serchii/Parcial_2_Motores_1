using UnityEngine;

public class PanelDebug : MonoBehaviour
{
    private void OnDisable()
    {
        Debug.Log($"{gameObject.name} se desactivó");
    }

    private void OnEnable()
    {
        Debug.Log($"{gameObject.name} se activó");
    }
}