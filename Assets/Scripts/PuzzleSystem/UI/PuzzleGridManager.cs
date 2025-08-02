using UnityEngine;
using UnityEngine.UI;
using System;

public class PuzzleGridManager : MonoBehaviour
{
    [System.Serializable]
    public class PuzzleCellData
    {
        public PuzzlePieceType type;
        public int rotation; // 0 = 0°, 1 = 90°, etc.
    }

    [Header("Grid Settings")]
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GameObject[] piecePrefabs;
    [SerializeField] private int rows = 4;
    [SerializeField] private int columns = 4;
    [SerializeField] private float cellSize = 100f;
    [SerializeField] private PuzzleCellData[] initialLayout;
    [SerializeField] private Vector2Int entryPoint = new Vector2Int(0, 0);
    [SerializeField] private Vector2Int exitPoint = new Vector2Int(3, 3);
    [SerializeField] private bool isCompleted = false;
    [SerializeField] private int money = 0;

    [Header("Piezas Instanciadas")]
    [SerializeField] private Color initialPieceColor = new Color(0.647f, 0.165f, 0.165f, 1.0f);

    public bool IsCompleted => isCompleted;

    private ItemSlot[,] gridSlots;

    public event Action OnCompleted;
    public event Action<int> OnGiveReward;

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(columns * cellSize, rows * cellSize);
        rt.anchoredPosition = Vector2.zero;
        rt.pivot = new Vector2(0.5f, 0.5f);

        float offsetX = (columns - 1) * cellSize / 2f;
        float offsetY = (rows - 1) * cellSize / 2f;

        gridSlots = new ItemSlot[rows, columns];

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                // Crear el slot
                GameObject slot = Instantiate(slotPrefab, transform);
                RectTransform slotRT = slot.GetComponent<RectTransform>();
                slotRT.anchoredPosition = new Vector2(x * cellSize - offsetX, -(y * cellSize - offsetY));

                ItemSlot itemSlot = slot.GetComponent<ItemSlot>();
                gridSlots[y, x] = itemSlot;

                int index = y * columns + x;

                if (initialLayout != null && index < initialLayout.Length)
                {
                    PuzzleCellData cellData = initialLayout[index];
                    if (cellData.type != PuzzlePieceType.None)
                    {
                        GameObject piece = Instantiate(piecePrefabs[(int)cellData.type], slot.transform);
                        RectTransform pieceRT = piece.GetComponent<RectTransform>();
                        pieceRT.anchoredPosition = Vector2.zero;
                        PuzzlePieceRotatable rot = piece.GetComponent<PuzzlePieceRotatable>();
                        if (rot != null)
                        {
                            rot.SetRotation(cellData.rotation);
                            rot.SetRotatable(false); // seguir bloqueando
                        }

                        // Bloquear arrastre y rotación
                        DragDropInstance drag = piece.GetComponent<DragDropInstance>();
                        if (drag != null) drag.SetDraggable(false);

                        // Pintar pieza instanciada
                        PuzzlePieceVisual visual = piece.GetComponent<PuzzlePieceVisual>();
                        if (visual != null) visual.SetColor(initialPieceColor);
                    }
                }
            }
        }
    }

    public void ValidateFlow()
    {
        //Reiniciamos los colores
        ResetColors();

        //Verificamos si las piezas van del principio a fin
        bool[,] visited = new bool[rows, columns];
        bool success = Traverse(entryPoint.x, entryPoint.y, visited);

        //Verificamos si todas las piezas estan conectadas correctamente
        bool allPiecesConnected = true;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                if (gridSlots[y, x].transform.childCount == 0) continue;

                if (!visited[y, x])
                {
                    allPiecesConnected = false;
                    break;
                }
            }
        }

        //Damos por completado el puzzle si las validaciones fueron correctas
        if (success && visited[exitPoint.y, exitPoint.x] && allPiecesConnected)
        {
            Debug.Log("✅ El flujo es correcto y todas las piezas están conectadas!");
            isCompleted = true;
            OnCompleted?.Invoke();
            PuzzleCompleted();
        }
        else
        {
            Debug.Log("❌ El flujo es incorrecto o hay piezas desconectadas");
            isCompleted = false;
        }

        // Pintar visitados
        
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                if (gridSlots[y, x].transform.childCount == 0) continue;

                GameObject piece = gridSlots[y, x].transform.GetChild(0).gameObject;
                Color color = visited[y, x] ? Color.green : Color.red;

                PuzzlePieceVisual visual = piece.GetComponent<PuzzlePieceVisual>();
                if (visual != null)
                    visual.FlashInvalid(color,1f);
            }
        }
    }

    private bool Traverse(int x, int y, bool[,] visited)
    {
        if (x < 0 || x >= columns || y < 0 || y >= rows)
            return false;

        if (visited[y, x]) return false;

        ItemSlot slot = gridSlots[y, x];
        if (slot.transform.childCount == 0) return false;

        GameObject pieceObj = slot.transform.GetChild(0).gameObject;
        DragDropInstance drag = pieceObj.GetComponent<DragDropInstance>();
        PuzzlePieceRotatable rot = pieceObj.GetComponent<PuzzlePieceRotatable>();

        if (drag == null || rot == null) return false;

        visited[y, x] = true;
        var connections = PipeConnectionData.GetConnections(drag.pieceType, rot.GetRotation());

        foreach (Direction dir in connections)
        {
            int nx = x, ny = y;

            switch (dir)
            {
                case Direction.Up: ny--; break;
                case Direction.Right: nx++; break;
                case Direction.Down: ny++; break;
                case Direction.Left: nx--; break;
            }

            if (nx < 0 || nx >= columns || ny < 0 || ny >= rows)
                continue;

            ItemSlot neighborSlot = gridSlots[ny, nx];
            if (neighborSlot.transform.childCount == 0) continue;

            GameObject neighborObj = neighborSlot.transform.GetChild(0).gameObject;
            DragDropInstance neighborDrag = neighborObj.GetComponent<DragDropInstance>();
            PuzzlePieceRotatable neighborRot = neighborObj.GetComponent<PuzzlePieceRotatable>();

            if (neighborDrag == null || neighborRot == null) continue;

            var neighborConnections = PipeConnectionData.GetConnections(neighborDrag.pieceType, neighborRot.GetRotation());
            Direction opposite = GetOppositeDirection(dir);

            if (System.Array.Exists(neighborConnections, d => d == opposite))
            {
                Traverse(nx, ny, visited);
            }
        }

        return true;
    }

    private void ResetColors()
    {
        foreach (ItemSlot slot in gridSlots)
        {
            if (slot.transform.childCount == 0) continue;

            GameObject piece = slot.transform.GetChild(0).gameObject;
            PuzzlePieceVisual visual = piece.GetComponent<PuzzlePieceVisual>();
            if (visual != null)
                visual.ResetColor();
        }
    }


    private Direction GetOppositeDirection(Direction dir)
    {
        return dir switch
        {
            Direction.Up => Direction.Down,
            Direction.Right => Direction.Left,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            _ => dir
        };
    }

    void PuzzleCompleted()
    {
        OnGiveReward?.Invoke(money);
    }
}
