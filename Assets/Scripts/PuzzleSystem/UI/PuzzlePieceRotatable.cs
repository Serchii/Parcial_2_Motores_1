using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzlePieceRotatable : MonoBehaviour, IPointerClickHandler
{
    private RectTransform rectTransform;
    private int rotationStep = 0;
    [SerializeField] bool isRotatable = true;

    public bool IsRotatable => isRotatable;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!isRotatable) return;
        rotationStep = (rotationStep + 1) % 4;
        rectTransform.rotation = Quaternion.Euler(0, 0, -90 * rotationStep);
    }

    public int GetRotation() => rotationStep;

    public void SetRotatable(bool value)
    {
        isRotatable = value;
    }

    public void SetRotation(int step)
    {
        rotationStep = step % 4;
        rectTransform.rotation = Quaternion.Euler(0, 0, -90 * rotationStep);
    }
}
