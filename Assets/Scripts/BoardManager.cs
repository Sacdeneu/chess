using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public int rows = 5;
    public int cols = 5;

    public GameObject tilePrefab;
    public Transform tilesParent;

    public GameObject playerPiecePrefab;
    public GameObject enemyKnightPrefab;

    public Tile[,] tiles;

    public void ClearHighlights()
    {
        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                tiles[x, y].Unhighlight();
            }
        }
    }

    public void GenerateBoard()
    {
        tiles = new Tile[rows, cols];

        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                GameObject tileObj = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity, tilesParent);
                Tile tile = tileObj.GetComponent<Tile>();
                tile.Init(x, y);
                tiles[x, y] = tile;

                SpriteRenderer sr = tileObj.GetComponentInChildren<SpriteRenderer>();
                if (sr != null)
                {
                    sr.color = (x + y) % 2 == 0 ? Color.white : Color.black;
                }
                else
                {
                    Debug.LogWarning("Tile prefab missing SpriteRenderer!");
                }
            }
        }
    }

    public void SpawnPlayerPiece(int x, int y)
    {
        GameObject pieceObj = Instantiate(playerPiecePrefab, tiles[x, y].transform.position, Quaternion.identity);
        KnightPiece piece = pieceObj.GetComponent<KnightPiece>();
        piece.x = x;
        piece.y = y;
        tiles[x, y].PlacePiece(piece);

        GameManager.Instance.playerPieces.Add(piece);
    }

    public void SpawnEnemyKnight(int x, int y)
    {
        GameObject pieceObj = Instantiate(enemyKnightPrefab, tiles[x, y].transform.position, Quaternion.identity);
        EnemyKnightPiece piece = pieceObj.GetComponent<EnemyKnightPiece>();
        piece.x = x;
        piece.y = y;
        tiles[x, y].PlacePiece(piece);

        GameManager.Instance.enemyPieces.Add(piece);
    }

    public bool IsValidPosition(int x, int y)
    {
        return x >= 0 && x < rows && y >= 0 && y < cols;
    }
}
