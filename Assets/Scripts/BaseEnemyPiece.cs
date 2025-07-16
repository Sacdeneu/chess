using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemyPiece : Piece
{
    protected virtual void Start()
    {
        isPlayer = false;
    }

    public void DoEnemyAction()
    {
        Tile[,] tiles = GameManager.Instance.boardManager.tiles;
        Vector2Int target = ChooseMove(tiles);

        if (target.x == x && target.y == y) return;

        // Si le mouvement cible une pièce du joueur, on la détruit
        if (tiles[target.x, target.y].isOccupied)
        {
            Piece targetPiece = tiles[target.x, target.y].occupyingPiece;
            if (targetPiece != null && targetPiece.isPlayer)
            {
                GameManager.Instance.playerPieces.Remove((BasePlayerPiece)targetPiece);
                Destroy(targetPiece.gameObject);
            }
        }

        tiles[x, y].RemovePiece();
        transform.position = new Vector3(target.x, target.y, -1);
        x = target.x;
        y = target.y;
        tiles[x, y].PlacePiece(this);
    }

    public Vector2Int ChooseMove(Tile[,] board)
    {
        List<Vector2Int> moves = GetAvailableMoves(board);

        // Prio 1 : capturer une pièce du joueur
        foreach (var move in moves)
        {
            if (board[move.x, move.y].isOccupied &&
                board[move.x, move.y].occupyingPiece.isPlayer)
            {
                return move;
            }
        }

        // Prio 2 : juste bouger si possible
        if (moves.Count > 0)
            return moves[0];

        // Sinon, ne bouge pas
        return new Vector2Int(x, y);
    }
}
