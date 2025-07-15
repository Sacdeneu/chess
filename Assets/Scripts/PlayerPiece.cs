using System.Collections.Generic;
using UnityEngine;

public class PlayerPiece : Piece
{
    private BoardManager board;
    private bool isDragging = false;
    private Vector3 offset;
    private Vector3 originalPosition;
    private int originalX, originalY;

    private void Start()
    {
        isPlayer = true;
        board = FindObjectOfType<BoardManager>();
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.currentTurn != GameManager.Turn.Player)
            return;

        isDragging = true;

        originalPosition = transform.position;
        originalX = x;
        originalY = y;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.position - new Vector3(mouseWorldPos.x, mouseWorldPos.y, transform.position.z);

        board.ClearHighlights();

        List<Vector2Int> moves = GetAvailableMoves(board.tiles);
        foreach (var move in moves)
        {
            board.tiles[move.x, move.y].Highlight(() =>
            {
                TryMove(move.x, move.y);
            });
        }
    }

    private void OnMouseDrag()
    {
        if (!isDragging)
            return;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetPos = new Vector3(mouseWorldPos.x, mouseWorldPos.y, transform.position.z) + offset;

        float angle = Mathf.Sin(Time.time * 10f) * 5f;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        transform.position = targetPos;
    }

    private void OnMouseUp()
    {
        if (!isDragging)
            return;

        isDragging = false;

        transform.rotation = Quaternion.identity;

        Tile closestTile = null;
        float minDist = float.MaxValue;
        foreach (var tile in board.tiles)
        {
            float dist = Vector3.Distance(transform.position, tile.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closestTile = tile;
            }
        }

        List<Vector2Int> validMoves = GetAvailableMoves(board.tiles);
        bool isValidMove = false;
        Vector2Int targetCoords = new Vector2Int();

        foreach (var move in validMoves)
        {
            if (closestTile.x == move.x && closestTile.y == move.y)
            {
                isValidMove = true;
                targetCoords = move;
                break;
            }
        }

        if (isValidMove)
        {
            TryMove(targetCoords.x, targetCoords.y);
        }
        else
        {
            transform.position = originalPosition;
            x = originalX;
            y = originalY;
        }

        board.ClearHighlights();
    }

    public override List<Vector2Int> GetAvailableMoves(Tile[,] board)
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        int boardWidth = board.GetLength(0);
        int boardHeight = board.GetLength(1);

        int newX = x;
        int newY = y + 1;

        if (newY < boardHeight && !board[newX, newY].isOccupied)
        {
            moves.Add(new Vector2Int(newX, newY));
        }

        if (newX + 1 < boardWidth && newY < boardHeight && board[newX + 1, newY].isOccupied && !board[newX + 1, newY].occupyingPiece.isPlayer)
        {
            moves.Add(new Vector2Int(newX + 1, newY));
        }

        if (newX - 1 >= 0 && newY < boardHeight && board[newX - 1, newY].isOccupied && !board[newX - 1, newY].occupyingPiece.isPlayer)
        {
            moves.Add(new Vector2Int(newX - 1, newY));
        }

        return moves;
    }

    private void TryMove(int targetX, int targetY)
    {
        Debug.Log("TryMove to " + targetX + "," + targetY);

        Tile[,] tiles = board.tiles;

        if (tiles[targetX, targetY].isOccupied)
        {
            Piece target = tiles[targetX, targetY].occupyingPiece;
            if (!target.isPlayer)
            {
                GameManager.Instance.enemyPieces.Remove((EnemyPiece)target);
                Destroy(target.gameObject);
            }
        }

        tiles[x, y].RemovePiece();
        transform.position = board.tiles[targetX, targetY].transform.position;
        x = targetX;
        y = targetY;
        tiles[x, y].PlacePiece(this);

        board.ClearHighlights();
        GameManager.Instance.NotifyPlayerActionUsed();
    }
}
