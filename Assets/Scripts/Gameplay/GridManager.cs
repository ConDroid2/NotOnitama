using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private Tile _tilePrefab;

    // References
    [Header("Object References")]
    public Grid Grid;
    [SerializeField] private GameObject _hoverIndicator;

    // Memeber variables
    public Dictionary<Vector3Int, Tile> TileDictionary = new Dictionary<Vector3Int, Tile>();
    private Vector3Int _currentHoveredCell;

    // Events
    public System.Action<Tile> OnTileClicked;

    private void Update()
    {
        // The cell the mouse is over
        Vector3Int hoveredCell = Grid.WorldToCell(MouseWorldPos());

        // If hovering a new cell
        if (hoveredCell != _currentHoveredCell)
        {
            // If in bounds, move the indicator to the cell
            if (CoordIsInBounds(hoveredCell.x, hoveredCell.y))
            {
                _hoverIndicator.transform.position = Grid.GetCellCenterWorld(hoveredCell);
                _currentHoveredCell = hoveredCell;
                _hoverIndicator.SetActive(true);
            }
            // If not in bounds, hide the indicator
            else
            {
                _hoverIndicator.SetActive(false);
            }
        }

        // If we clicked a game tile, fire the event
        if (Input.GetMouseButtonDown(0))
        {
            if(TileDictionary.ContainsKey(hoveredCell))
            {
                OnTileClicked?.Invoke(TileDictionary[hoveredCell]);
            }
        }
    }

    // Generate Grid of a given size 
    public void SetupGrid()
    {
        for(int x = 0; x < 5; x++)
        {
            for(int y = 0; y < 5; y++)
            {
                // Create the tile
                Tile newTile = Instantiate(_tilePrefab, parent: Grid.transform);

                // Figure out where in the grid it goes, put it there
                Vector3Int gridPosition = new Vector3Int(x, y, 0);
                newTile.GridPos = gridPosition;
                TileDictionary.Add(gridPosition, newTile);
                newTile.transform.position = Grid.GetCellCenterWorld(gridPosition);
            }
        }

        // Set the master's seats
        TileDictionary[new Vector3Int(2, 0, 0)].SetMasterSeat("Blue");
        TileDictionary[new Vector3Int(2, 4, 0)].SetMasterSeat("Red");

        // Move the camera to the right spot
        float cameraXPos = (0 + 5) / 2f;
        float cameraYPos = (0 + 5) / 2f;
        Camera.main.transform.position = new Vector3(cameraXPos, cameraYPos, Camera.main.transform.position.z);
    }

    // Set the piece to a given tile
    public void SetPieceToTile(int x, int y, Piece piece)
    {
        if(CoordIsInBounds(x, x) == false) { return; }

        TileDictionary[new Vector3Int(x, y, 0)].SetPiece(piece);
    }

    // Highlight and make selectable all tiles that the given move card would allow for the given piece
    public void ShowValidMoves(Player currentPlayer, Tile selectedTile, MoveCardController moveCard)
    {
        ResetTiles();

        // Figure out if the card is "upside down"
        int invertedModifier = moveCard.IsInverted ? -1 : 1;

        // Go through each move option in the move card
        foreach (Vector2Int move in moveCard.MoveCardData.moveOption)
        {
            int xPos = selectedTile.GridPos.x + (move.x * invertedModifier);
            int yPos = selectedTile.GridPos.y + (move.y * invertedModifier);

            Vector3Int gridPos = new Vector3Int(xPos, yPos, 0);

            if (TileDictionary.ContainsKey(gridPos))
            {
                // If the tile we're checking has no piece or the current player doesn't own the piece
                if (TileDictionary[gridPos].Piece == null || currentPlayer.OwnsPiece(TileDictionary[gridPos].Piece) == false)
                {
                    TileDictionary[gridPos].SetSelectable(true);
                }
            }
        }
    }

    // Make all tiles not selectable
    public void ResetTiles()
    {
        foreach(Tile tile in TileDictionary.Values)
        {
            tile.SetSelectable(false);
        }
    }

    // Utility Functions
    // Return true if the given coordinate is in the grid
    private bool CoordIsInBounds(int x, int y)
    {
        return x >= 0 && x < 5 && y >= 0 && y < 5;
    }

    // Return where the mouse is in world position
    private Vector3 MouseWorldPos()
    {
        Vector3 mousePos = Input.mousePosition;

        mousePos.z = Camera.main.WorldToScreenPoint(transform.position).z;

        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
