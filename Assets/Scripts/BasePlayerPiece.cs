using UnityEngine;

public abstract class BasePlayerPiece : Piece
{
    protected BoardManager board;
    private bool isDragging = false;
    private Vector3 offset;
    private Vector3 originalPosition;
    private int originalX, originalY;

    protected virtual void Start()
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
        var moves = GetAvailableMoves(board.tiles);
        foreach (var move in moves)
        {
            board.tiles[move.x, move.y].Highlight(() => TryMove(move.x, move.y));
        }
    }

    private void OnMouseDrag()
    {
        if (!isDragging) return;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mouseWorldPos.x, mouseWorldPos.y, transform.position.z) + offset;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * 10f) * 5f);
    }

    private void OnMouseUp()
    {
        if (!isDragging) return;
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

        var validMoves = GetAvailableMoves(board.tiles);
        foreach (var move in validMoves)
        {
            if (closestTile.x == move.x && closestTile.y == move.y)
            {
                TryMove(move.x, move.y);
                board.ClearHighlights();
                return;
            }
        }

        // Invalid move → reset position
        transform.position = originalPosition;
        x = originalX;
        y = originalY;
        board.ClearHighlights();
    }

    protected void TryMove(int targetX, int targetY)
    {
        var tiles = board.tiles;

        if (tiles[targetX, targetY].isOccupied)
        {
            var target = tiles[targetX, targetY].occupyingPiece;
            if (!target.isPlayer)
            {
                GameManager.Instance.enemyPieces.Remove((EnemyKnightPiece)target);
                Destroy(target.gameObject);
            }
        }

        tiles[x, y].RemovePiece();
        transform.position = new Vector3(targetX, targetY, -1);
        x = targetX;
        y = targetY;
        tiles[x, y].PlacePiece(this);

        GameManager.Instance.NotifyPlayerActionUsed();
    }
}