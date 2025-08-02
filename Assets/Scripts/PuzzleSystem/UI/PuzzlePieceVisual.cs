using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PuzzlePieceVisual : MonoBehaviour
{
    [SerializeField] private Image pieceImage;

    private Color originalColor;

    private void Awake()
    {
        if (pieceImage == null)
            pieceImage = GetComponent<Image>();

        originalColor = pieceImage.color;
    }

    public void SetColor(Color newColor)
    {
        originalColor = newColor;
        pieceImage.color = newColor;
    }

    public void ResetColor()
    {
        StopAllCoroutines();
        pieceImage.color = originalColor;
    }

    public void FlashInvalid(Color invalidColor, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(FlashInvalidCoroutine(invalidColor, duration));
    }

    private IEnumerator FlashInvalidCoroutine(Color invalidColor, float duration)
    {
        pieceImage.color = invalidColor;
        yield return new WaitForSeconds(duration);
        pieceImage.color = originalColor;
        Debug.Log("ColorDetenido");
    }
}