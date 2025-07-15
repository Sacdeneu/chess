using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int x;
    public int y;

    public bool isOccupied = false;
    public Piece occupyingPiece;

    public GameObject highlightObject;

    private Action onClickAction;
    public void Init(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    private void Start()
    {
        if (highlightObject != null)
            highlightObject.SetActive(false);
    }

    public void PlacePiece(Piece piece)
    {
        occupyingPiece = piece;
        isOccupied = true;
    }

    public void RemovePiece()
    {
        occupyingPiece = null;
        isOccupied = false;
    }

    public void Highlight(Action onClick)
    {
        if (highlightObject != null)
            highlightObject.SetActive(true);

        onClickAction = onClick;
    }

    public void Unhighlight()
    {
        if (highlightObject != null)
            highlightObject.SetActive(false);

        onClickAction = null;
    }

    private void OnMouseDown()
    {
        if (highlightObject != null && highlightObject.activeSelf && onClickAction != null)
        {
            onClickAction.Invoke();
        }
    }
}
