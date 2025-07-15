using System.Collections;
using UnityEngine;

public enum TurnPhase { Player, Enemy }

public class TurnManager : MonoBehaviour
{
    public TurnPhase currentTurn = TurnPhase.Player;

    public void EndTurn()
    {
        if (currentTurn == TurnPhase.Player)
        {
            currentTurn = TurnPhase.Enemy;
            StartCoroutine(EnemyTurn());
        }
        else
        {
            currentTurn = TurnPhase.Player;
        }
    }

    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1f);
        EndTurn();
    }
}
