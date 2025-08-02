using UnityEngine;
using UnityEngine.UI;

public class PuzzlePiece : MonoBehaviour
{/*
    public PuzzlePieceType pipeType;

    private int rotationStep = 0; // 0 = 0°, 1 = 90°, 2 = 180°, 3 = 270°

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void RotatePiece()
    {
        rotationStep = (rotationStep + 1) % 4;
        rectTransform.rotation = Quaternion.Euler(0, 0, -90 * rotationStep);
    }

    public int GetRotation()
    {
        return rotationStep;
    }

    public PuzzlePieceType GetPipeType()
    {
        return pipeType;
    }

    public void SetPipeType(PuzzlePieceType newType, Sprite newSprite)
    {
        pipeType = newType;
        rotationStep = 0;
        transform.rotation = Quaternion.identity;

        Image image = GetComponent<Image>();
        image.sprite = newSprite;
        image.enabled = newType != PuzzlePieceType.None;
    }*/
}
