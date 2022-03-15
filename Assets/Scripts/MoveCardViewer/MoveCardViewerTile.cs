using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The tiles that are used on the move cards
public class MoveCardViewerTile : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] SpriteRenderer spriteRenderer;

    [Header("Settings")]
    public Color canMoveToColorRed;
    public Color canMoveToColorBlue;
    public Color canMoveToColorGrey;
    public Color defaultColor;
    public Color startingTileColor;

    // Other variables
    public Vector2Int gridPosition;

    public bool isStartingTile = false;

    // Set tile to default color
    public void ResetColor()
    {
        spriteRenderer.color = defaultColor;
    }

    // Set the tile to the correct color
    public void SetCanMoveTo(MoveCard.Color tileColor)
    {
        if (tileColor == MoveCard.Color.Blue)
            spriteRenderer.color = canMoveToColorBlue;

        else if (tileColor == MoveCard.Color.Red)
            spriteRenderer.color = canMoveToColorRed;

        else if (tileColor == MoveCard.Color.Grey)
            spriteRenderer.color = canMoveToColorGrey;
    }

    // Set this tile as the starting (middle) tile for the grid
    public void SetStartingTile()
    {
        isStartingTile = true;
        spriteRenderer.color = startingTileColor;
    }
}
