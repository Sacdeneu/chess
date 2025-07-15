using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public int rows = 5;
    public int cols = 5;

    public GameObject tilePrefab;
    public Transform tilesParent;
    public GameObject playerPiecePrefab;
    public GameObject enemyPiecePrefab;

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
                    if ((x + y) % 2 == 0)
                        sr.color = Color.white;
                    else
                        sr.color = Color.black;
                }
                else
                {
                    Debug.LogWarning("Tile prefab missing SpriteRenderer component in children!");
                }

            }
        }
    }

    public bool IsValidPosition(int x, int y)
    {
        return x >= 0 && x < rows && y >= 0 && y < cols;
    }
}