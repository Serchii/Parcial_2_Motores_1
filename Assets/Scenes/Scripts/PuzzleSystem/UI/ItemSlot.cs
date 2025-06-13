using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] bool isOccupied = false;
    public GameObject currentPiece;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null || isOccupied || !eventData.pointerDrag.GetComponent<DragDropInstance>().IsDraggable)
            return;

        currentPiece = eventData.pointerDrag;
        currentPiece.transform.SetParent(transform);
        currentPiece.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        isOccupied = true;
    }

    public PuzzlePieceType GetPieceType()
    {
        if (currentPiece == null) return PuzzlePieceType.None;
        return currentPiece.GetComponent<DragDropInstance>().pieceType;
    }

    public int GetRotation()
    {
        if (currentPiece == null) return 0;
        return currentPiece.GetComponent<PuzzlePieceRotatable>()?.GetRotation() ?? 0;
    }

    public void SetDebugColor(Color color)
    {
        GetComponent<Image>().color = color;
    }
}
