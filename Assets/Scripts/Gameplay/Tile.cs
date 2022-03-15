using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tile : MonoBehaviour
{
    [Header("Adjustable Settings")]
    public Color SelectableColor;
    public Color DefaultColor;

    [Header("Object References")]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject _redMasterSeat;
    [SerializeField] private GameObject _blueMasterSeat;

    // Hidden Settings
    [HideInInspector] public bool IsRedMasterSeat = false;
    [HideInInspector] public bool IsBlueMasterSeat = false;
    [HideInInspector] public bool IsSelectable = false;

    // Variables
    [HideInInspector] public Piece Piece { get; private set; }
    [HideInInspector] public Vector3Int GridPos;
    public System.Action<Piece> OnPieceDestroyed;
    public System.Action<Tile, Piece> OnPieceMovedToTile;

    // Sets a new piece as this tile's piece, destroys old one if there is one
    public void SetPiece(Piece newPiece)
    {
        if(Piece != null)
        {
            OnPieceDestroyed?.Invoke(Piece);
            Destroy(Piece.gameObject);
        }

        Piece = newPiece;
        Piece.transform.position = transform.position;
        OnPieceMovedToTile?.Invoke(this, Piece);
    }

    // Sets the tile's piece to null
    public void PieceMoved()
    {
        Piece = null;
    }

    // Makes the tile selectable (or not)
    public void SetSelectable(bool selectable)
    {
        IsSelectable = selectable;
        if(selectable == true)
        {
            _spriteRenderer.color = SelectableColor;
        }
        else
        {
            _spriteRenderer.color = DefaultColor;
        }
    }

    // Sets this tile as one of the Master's seats
    public void SetMasterSeat(string color)
    {
        if(color == "Blue")
        {
            IsBlueMasterSeat = true;
            _blueMasterSeat.SetActive(true);
        }
        else if(color == "Red")
        {
            IsRedMasterSeat = true;
            _redMasterSeat.SetActive(true);
        }
    }
}
