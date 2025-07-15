using UnityEngine;

public enum BonusType { AddPiece, UpgradePiece, Heal }

public class Bonus : MonoBehaviour
{
    public BonusType bonusType;

    public void Apply(Piece piece)
    {
        switch (bonusType)
        {
            case BonusType.AddPiece:
                break;
            case BonusType.UpgradePiece:
                break;
            case BonusType.Heal:
                break;
        }

        Destroy(gameObject);
    }
}
