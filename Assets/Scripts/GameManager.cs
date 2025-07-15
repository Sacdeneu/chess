using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum Turn { Player, Enemy }
    public Turn currentTurn = Turn.Player;

    public BoardManager boardManager;

    public List<PlayerPiece> playerPieces = new List<PlayerPiece>();
    public List<EnemyPiece> enemyPieces = new List<EnemyPiece>();

    private bool actionInProgress = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        boardManager.GenerateBoard();
        SpawnInitialPieces();
        StartPlayerTurn();
    }

    void SpawnInitialPieces()
    {
        SpawnPlayerPiece(2, 0);

        SpawnEnemyPiece(2, 4);
    }

    public void SpawnPlayerPiece(int x, int y)
    {
        Vector3 worldPos = boardManager.tiles[x, y].transform.position;
        worldPos.z = -1;

        GameObject go = Instantiate(boardManager.playerPiecePrefab, worldPos, Quaternion.identity);
        PlayerPiece piece = go.GetComponent<PlayerPiece>();
        piece.x = x;
        piece.y = y;
        boardManager.tiles[x, y].PlacePiece(piece);
        playerPieces.Add(piece);
    }

    public void SpawnEnemyPiece(int x, int y)
    {
        Vector3 worldPos = boardManager.tiles[x, y].transform.position;

        GameObject go = Instantiate(boardManager.enemyPiecePrefab, worldPos, Quaternion.identity);
        EnemyPiece piece = go.GetComponent<EnemyPiece>();
        piece.x = x;
        piece.y = y;
        boardManager.tiles[x, y].PlacePiece(piece);
        enemyPieces.Add(piece);
    }

    public void EndTurn()
    {
        if (currentTurn == Turn.Player)
        {
            currentTurn = Turn.Enemy;
            StartCoroutine(EnemyTurnRoutine());
        }
        else
        {
            currentTurn = Turn.Player;
            StartPlayerTurn();
        }
    }

    void StartPlayerTurn()
    {
        boardManager.ClearHighlights();
        actionInProgress = false;
    }

    System.Collections.IEnumerator EnemyTurnRoutine()
    {
        boardManager.ClearHighlights();
        yield return new WaitForSeconds(0.5f);

        foreach (var enemy in enemyPieces)
        {
            if (enemy == null) continue;

            enemy.DoEnemyAction();
            yield return new WaitForSeconds(0.3f);
        }

        CheckEndGame();
        EndTurn();
    }

    public void NotifyPlayerActionUsed()
    {
        if (!actionInProgress)
        {
            actionInProgress = true;
            Invoke("EndTurn", 0.5f);
        }
    }

    public void CheckEndGame()
    {
        playerPieces.RemoveAll(p => p == null);
        enemyPieces.RemoveAll(e => e == null);

        if (playerPieces.Count == 0)
        {
            Debug.Log("Game Over! Enemy Wins.");
        }

        if (enemyPieces.Count == 0)
        {
            Debug.Log("Victory! Player Wins.");
        }
    }
}