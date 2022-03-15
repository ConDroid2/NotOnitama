using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    // Variables only really used for win condition checking
    public bool IsMaster = false;
    public Color PieceColor;

    public enum Color
    {
        Blue = 1,
        Red = 2
    }
}
