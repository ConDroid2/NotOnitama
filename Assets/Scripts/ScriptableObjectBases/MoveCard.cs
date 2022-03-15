using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MoveCard")]
public class MoveCard : ScriptableObject
{
    public string moveName;
    public List<Vector2Int> moveOption;
    // Color that determines which player is first
    public Color AssociatedColor;
    // Color of the tiles you can move to (on the move card)
    public Color TileColor;

    public enum Color
    {
        Red = 1,
        Blue = 2,
        Grey = 3
    }
}
