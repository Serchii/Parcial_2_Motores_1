using System.Collections.Generic;

public static class PipeConnectionData
{
    private static readonly Dictionary<PuzzlePieceType, Direction[][]> connectionMap = new()
    {
        {
            PuzzlePieceType.Straight, new Direction[][]
            {
                new[] { Direction.Up, Direction.Down },      // rot 0
                new[] { Direction.Left, Direction.Right },   // rot 90
                new[] { Direction.Up, Direction.Down },      // rot 180
                new[] { Direction.Left, Direction.Right }    // rot 270
            }
        },
        {
            PuzzlePieceType.Curve, new Direction[][]
            {
                new[] { Direction.Left, Direction.Up },      // rot 0
                new[] { Direction.Up, Direction.Right },     // rot 90
                new[] { Direction.Right, Direction.Down },   // rot 180
                new[] { Direction.Down, Direction.Left }     // rot 270
            }
        },
        {
            PuzzlePieceType.TShape, new Direction[][]
            {
                new[] { Direction.Left, Direction.Right, Direction.Down },  // rot 0
                new[] { Direction.Up, Direction.Left, Direction.Down },     // rot 90
                new[] { Direction.Up, Direction.Left, Direction.Right },    // rot 180
                new[] { Direction.Up, Direction.Right, Direction.Down }     // rot 270
            }
        },
        {
            PuzzlePieceType.Cross, new Direction[][]
            {
                new[] { Direction.Up, Direction.Right, Direction.Down, Direction.Left },
                new[] { Direction.Up, Direction.Right, Direction.Down, Direction.Left },
                new[] { Direction.Up, Direction.Right, Direction.Down, Direction.Left },
                new[] { Direction.Up, Direction.Right, Direction.Down, Direction.Left }
            }
        },
        {
            PuzzlePieceType.Entry, new Direction[][]
            {
                new[] { Direction.Down},    // rot 0
                new[] { Direction.Left},    // rot 90
                new[] { Direction.Up},      // rot 180
                new[] { Direction.Right }   // rot 270
            }
        },
        {
            PuzzlePieceType.Exit, new Direction[][]
            {
                new[] { Direction.Up},      // rot 0
                new[] { Direction.Right },  // rot 90
                new[] { Direction.Down },   // rot 180
                new[] { Direction.Left }    // rot 270
            }
        }
    };

    public static Direction[] GetConnections(PuzzlePieceType type, int rotation)
    {
        if (!connectionMap.ContainsKey(type))
            return new Direction[0];

        return connectionMap[type][rotation % 4];
    }
}
