using UnityEngine;

[System.Serializable]
public class PieceStock
{
    public PuzzlePieceType pieceType;
    public int initialCount;
    [HideInInspector] public int currentCount;
}