using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDropInstance : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] RectTransform rectTransform;
    [SerializeField] Canvas canvas;
    [SerializeField] CanvasGroup canvasGroup;

    [SerializeField] bool isDraggable = true;

    public bool IsDraggable => isDraggable;

    public PuzzlePieceType pieceType;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!isDraggable) return;
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;

        transform.SetParent(canvas.transform); // para que no esté limitado por el botón o celda
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(!isDraggable) return;
        rectTransform.position = eventData.position;
        Debug.Log(gameObject.name);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(!isDraggable) return;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if (!eventData.pointerEnter || !eventData.pointerEnter.GetComponent<ItemSlot>())
        {
            Destroy(gameObject); // si no se suelta sobre un slot
        }
        else
        {
            // Anclar en el centro del slot
            transform.SetParent(eventData.pointerEnter.transform);
            rectTransform.anchoredPosition = Vector2.zero;
        }
    }

    public void SetDraggable(bool value)
    {
        isDraggable = value;
    }
}
