using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    public int x, y;
    public bool isPlayer;
    public abstract List<Vector2Int> GetAvailableMoves(Tile[,] board);
    public virtual void MoveTo(int targetX, int targetY)
    {
        x = targetX;
        y = targetY;
        transform.position = new Vector3(x, y, 0);
    }
}