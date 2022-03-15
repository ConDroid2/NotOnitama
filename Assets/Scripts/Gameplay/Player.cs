using System.Collections;
using System.Collections.Generic;

public class Player
{
    // Pieces owned by player
    public List<Piece> Pieces;
    // Move cards
    public MoveCardController LeftCard;
    public MoveCardController RightCard;

    public Player(string color)
    {
        Pieces = new List<Piece>();
    }

    // Check if player owns a given piece
    public bool OwnsPiece(Piece piece)
    {
        return Pieces.Contains(piece);
    }

    // Check if player owns a given move card
    public bool OwnsMoveCard(MoveCardController moveCard)
    {
        return moveCard == LeftCard || moveCard == RightCard;
    }
}
