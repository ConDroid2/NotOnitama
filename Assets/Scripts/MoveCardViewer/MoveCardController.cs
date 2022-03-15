using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class MoveCardController : MonoBehaviour, IPointerClickHandler
{
    [Header("Settings")]
    [SerializeField] private bool InitializeOnAwake = false;
    [SerializeField] private float _tileScale = 0.2f; // Used to put the tiles in the right spot

    [Header("References")]
    [SerializeField] private List<MoveCardViewerTile> _moveCardTiles = new List<MoveCardViewerTile>(); 
    [SerializeField] private TextMeshPro nameText;
    [SerializeField] private GameObject gridParent;

    // Variables
    Dictionary<Vector2Int, MoveCardViewerTile> _moveCardTilesMap = new Dictionary<Vector2Int, MoveCardViewerTile>();
    private MoveCardViewerTile _startingTile;
    [HideInInspector] public MoveCard MoveCardData { get; private set; }
    private bool _showingInverted = false;
    public bool IsInverted => _showingInverted;

    // Events
    public System.Action<MoveCardController> OnMoveCardClicked;

    private void Awake()
    {
        if (InitializeOnAwake) Initialize();
    }

    public void Initialize()
    {
        // Calculate where the first tile should go (bottom left corner)
        float xStartPos = gridParent.transform.position.x - (2 * _tileScale);
        float yStartPos = gridParent.transform.position.y - (2 * _tileScale);

        // Loop through the tiles
        int col = -1;
        for(int i = 0; i < _moveCardTiles.Count; i++)
        {
            int row = i % 5;
            if(row == 0)
            {
                col++;
            }

            MoveCardViewerTile currentTile = _moveCardTiles[i];

            // Put the tile in the right palce
            float newXPos = xStartPos + (col * _tileScale);
            float newYPos = yStartPos + (row * _tileScale);
            currentTile.transform.position = new Vector3(newXPos, newYPos, currentTile.transform.position.z);

            // Add the tile to the map with it's grid coordinates as the key
            currentTile.gridPosition = new Vector2Int(col, row);
            _moveCardTilesMap.Add(new Vector2Int(col, row), currentTile);
        }

        _startingTile = _moveCardTilesMap[new Vector2Int(2, 2)];
        _startingTile.SetStartingTile();
    }

    private void ResetTiles()
    {
        foreach(MoveCardViewerTile tile in _moveCardTilesMap.Values)
        {
            if (tile.isStartingTile == false)
            {
                tile.ResetColor();
            }
        }
    }

    public void SetMoveCard(MoveCard moveCard, bool inverted)
    {
        int invertedModifier = inverted ? -1 : 1;

        ResetTiles();
        foreach(Vector2Int move in moveCard.moveOption)
        {
            int xPos = _startingTile.gridPosition.x + (move.x * invertedModifier);
            int yPos = _startingTile.gridPosition.y + (move.y * invertedModifier);

            Vector2Int newGridPos = new Vector2Int(xPos, yPos);

            if (_moveCardTilesMap.ContainsKey(newGridPos))
            {
                _moveCardTilesMap[newGridPos].SetCanMoveTo(moveCard.TileColor);
            }
        }

        nameText.text = moveCard.moveName;

        MoveCardData = moveCard;
        _showingInverted = inverted;
    }

    // Flip whether or not the grid is inverted
    public void FlipGrid()
    {
        SetMoveCard(MoveCardData, !_showingInverted);
    }

    // When the card is clicked
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnMoveCardClicked?.Invoke(this);
        }
    }
}
