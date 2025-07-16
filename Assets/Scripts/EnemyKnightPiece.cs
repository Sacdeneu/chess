using System.Collections.Generic;
using UnityEngine;

public class EnemyKnightPiece : BaseEnemyPiece
{
    public override List<Vector2Int> GetAvailableMoves(Tile[,] board)
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        Vector2Int[] offsets = new Vector2Int[]
        {
            new Vector2Int(+2, +1), new Vector2Int(+1, +2),
            new Vector2Int(-1, +2), new Vector2Int(-2, +1),
            new Vector2Int(-2, -1), new Vector2Int(-1, -2),
            new Vector2Int(+1, -2), new Vector2Int(+2, -1)
        };

        foreach (var offset in offsets)
        {
            int nx = x + offset.x;
            int ny = y + offset.y;

            if (InBounds(nx, ny, board))
            {
                if (!board[nx, ny].isOccupied || board[nx, ny].occupyingPiece.isPlayer)
                {
                    moves.Add(new Vector2Int(nx, ny));
                }
            }
        }

        return moves;
    }

    private bool InBounds(int x, int y, Tile[,] board)
    {
        return x >= 0 && x < board.GetLength(0) && y >= 0 && y < board.GetLength(1);
    }
}

