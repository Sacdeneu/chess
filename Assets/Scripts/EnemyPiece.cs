using System.Collections.Generic;
using UnityEngine;

public class EnemyPiece : Piece
{
    private void Start()
    {
        isPlayer = false;
    }

    public override List<Vector2Int> GetAvailableMoves(Tile[,] board)
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        int boardWidth = board.GetLength(0);
        int boardHeight = board.GetLength(1);

        int newX = x;
        int newY = y - 1;

        if (newY >= 0 && !board[newX, newY].isOccupied)
        {
            moves.Add(new Vector2Int(newX, newY));
        }

        if (newX + 1 < boardWidth && newY >= 0 && board[newX + 1, newY].isOccupied && board[newX + 1, newY].occupyingPiece.isPlayer)
        {
            moves.Add(new Vector2Int(newX + 1, newY));
        }

        if (newX - 1 >= 0 && newY >= 0 && board[newX - 1, newY].isOccupied && board[newX - 1, newY].occupyingPiece.isPlayer)
        {
            moves.Add(new Vector2Int(newX - 1, newY));
        }

        return moves;
    }

    public void DoEnemyAction()
    {
        Tile[,] board = GameManager.Instance.boardManager.tiles;
        Vector2Int target = ChooseMove(board);

        if (target.x == x && target.y == y)
            return;

        if (board[target.x, target.y].isOccupied)
        {
            Piece targetPiece = board[target.x, target.y].occupyingPiece;
            if (targetPiece != null && targetPiece.isPlayer)
            {
                GameManager.Instance.playerPieces.Remove((PlayerPiece)targetPiece);
                Destroy(targetPiece.gameObject);
            }
        }

        board[x, y].RemovePiece();
        transform.position = new Vector3(target.x, target.y, 0);
        x = target.x;
        y = target.y;
        board[x, y].PlacePiece(this);
    }

    public Vector2Int ChooseMove(Tile[,] board)
    {
        List<Vector2Int> moves = GetAvailableMoves(board);

        foreach (var move in moves)
        {
            if (board[move.x, move.y].isOccupied && board[move.x, move.y].occupyingPiece.isPlayer)
                return move;
        }

        if (moves.Count > 0)
            return moves[0];

        return new Vector2Int(x, y);
    }
}
