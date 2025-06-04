using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableFromPanel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject dragPrefab;
    private GameObject instance;
    private RectTransform instanceRect;
    private Canvas canvas;

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        instance = Instantiate(dragPrefab, canvas.transform);
        instanceRect = instance.GetComponent<RectTransform>();
        instanceRect.position = eventData.position;

        // Cambiamos la pieza visualmente
        var thisImage = GetComponent<Image>();
        var instanceImage = instance.GetComponent<Image>();
        instanceImage.sprite = thisImage.sprite;

        // Marcar la instancia como el objeto arrastrado
        eventData.pointerDrag = instance;

        // Requiere CanvasGroup para que no interfiera en el raycast
        var cg = instance.GetComponent<CanvasGroup>();
        if (cg != null)
        {
            cg.blocksRaycasts = false;
            cg.alpha = 0.6f;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (instanceRect != null)
        {
            instanceRect.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Nada que hacer acá, la instancia se destruye o acomoda en DragDropInstance
    }
}
