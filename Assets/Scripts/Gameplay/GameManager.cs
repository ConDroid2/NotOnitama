using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [Header("Manager References")]
    [SerializeField] private SetupManager _setupManager;

    [Header("Object References")]
    public Transform BluePlayerLeftCardPos;
    public Transform BluePlayerRightCardPos;
    public Transform RedPlayerLeftCardPos;
    public Transform RedPlayerRightCardPos;
    public Transform CenterCardPos;
    [SerializeField] private GameObject _selectedTileIndicator;
    [SerializeField] private GameObject _selectedCardIndicator;

    public List<MoveCardController> MoveCards = new List<MoveCardController>();
    [SerializeField] private TurnChangeBannerController _bannerController;
    [SerializeField] private GameWonScreenController _gameWonScreen;

    /** Game Variables **/
    public Player BluePlayer;
    public Player RedPlayer;
    private Player _currentPlayer;

    [HideInInspector] public MoveCardController CenterCard;

    private Tile _selectedTile;
    private MoveCardController _selectedMoveCard;
    private GameState _gameState = GameState.WaitingForPlayerInput;

    [Header("Events")]
    public UnityEvent OnPieceMove;

    public enum GameState
    {
        WaitingForPlayerInput = 1,
        BlockInput = 2,
        GameWon = 3
    }

    private void Awake()
    {
        // Create the players
        BluePlayer = new Player("Blue");
        RedPlayer = new Player("Red");

        // Initialize the game
        _setupManager.Initialize(this);

        // Start the Game
        StartGame();
    }

    // Listeners
    public void HandleTileClicked(Tile tile)
    {
        if (_gameState != GameState.WaitingForPlayerInput) return;

        // If we clicked our own piece
        if (tile.Piece != null && _currentPlayer.OwnsPiece(tile.Piece))
        {
            _selectedTile = tile;
            _selectedTileIndicator.SetActive(true);
            _selectedTileIndicator.transform.position = _selectedTile.transform.position;
            ShowValidMoves();
        }

        // The player has picked a tile to move to
        else if (tile.IsSelectable && _selectedTile != null)
        {
            TakeTurn(tile);
        }
    }

    public void HandleMoveCardClicked(MoveCardController moveCard)
    {
        if (_gameState != GameState.WaitingForPlayerInput) return;

        if (_currentPlayer.OwnsMoveCard(moveCard))
        {
            _selectedMoveCard = moveCard;
            _selectedCardIndicator.transform.position = _selectedMoveCard.transform.position;
            _selectedCardIndicator.SetActive(true);

            ShowValidMoves();
        }
    }

    public void HandlePieceDestroyed(Piece piece)
    {
        // Only a master being destroyed matters
        if (piece.IsMaster == false) return;

        // If the master was blue, red wins
        if (piece.PieceColor == Piece.Color.Blue)
        {
            EndGame("Red");
        }

        // If the master was red, blue wins
        if (piece.PieceColor == Piece.Color.Red)
        {
            EndGame("Blue");
        }
    }

    public void HandlePieceMovedToTile(Tile tile, Piece piece)
    {
        // Only a master moving to a specific tile matters
        if (piece.IsMaster == false) return;

        // If blue master moving to red master seat, blue win
        if (piece.PieceColor == Piece.Color.Blue && tile.IsRedMasterSeat)
        {
            EndGame("Blue");
        }

        // If red master moving to blue master seat, red wins
        else if (piece.PieceColor == Piece.Color.Red && tile.IsBlueMasterSeat)
        {
            EndGame("Red");
        }
    }
    // End Listeners

    // Starts the game
    private void StartGame()
    {
        // Subscribe to the relevant events
        _setupManager.GridManager.OnTileClicked += HandleTileClicked;

        foreach (MoveCardController card in MoveCards)
        {
            card.OnMoveCardClicked += HandleMoveCardClicked;
        }

        // Decide whot he first player is
        if (CenterCard.MoveCardData.AssociatedColor == MoveCard.Color.Blue)
        {
            _currentPlayer = BluePlayer;
            _bannerController.SetBannerText("Blue Player's Turn");
        }
        else
        {
            _currentPlayer = RedPlayer;
            _bannerController.SetBannerText("Red Player's Turn");
            CenterCard.FlipGrid();
        }       
    }

    // Show what tiles can be moved to if we have a tile and move card
    public void ShowValidMoves()
    {
        if (_selectedTile == null || _selectedMoveCard == null) return;

        _setupManager.GridManager.ShowValidMoves(_currentPlayer, _selectedTile, _selectedMoveCard);

    }

    // Take the turn (move the piece to the selected spot
    public void TakeTurn(Tile moveTo)
    {
        _gameState = GameState.BlockInput;

        _setupManager.GridManager.ResetTiles();

        OnPieceMove?.Invoke();
        // Move the piece, then set up the next turn
        _selectedTile.Piece.transform.DOMove(moveTo.transform.position, 0.15f).OnComplete(() => 
            {   
                moveTo.SetPiece(_selectedTile.Piece);
                _selectedTile.PieceMoved();
                SetupNextTurn();
            });
    }

    // Sets up the next turn
    public void SetupNextTurn()
    {
        if (_gameState == GameState.GameWon) return;

        Vector3 usedCardPosition = _selectedMoveCard.transform.position;

        // Store which card was clicked (left or right)
        bool selectedCardIsLeftCard = _selectedMoveCard == _currentPlayer.LeftCard;

        // need to swap the center card and the selected card
        CenterCard.transform.DOMove(usedCardPosition, 0.25f);

        if (selectedCardIsLeftCard)
        {
            _currentPlayer.LeftCard = CenterCard;
        }
        else
        {
            _currentPlayer.RightCard = CenterCard;
        }
        
        
        // Move the selected card to the center, start the next turn
        _selectedMoveCard.transform.DOMove(CenterCardPos.position, 0.25f).OnComplete(() => { 

            // Set the Center Card reference, and flip the card
            CenterCard = _selectedMoveCard;
            CenterCard.FlipGrid();

            // Swap the player
            if (_currentPlayer == RedPlayer)
            {
                _currentPlayer = BluePlayer;
                _bannerController.SetBannerText("Blue Player's Turn");
            }
            else
            {
                _currentPlayer = RedPlayer;
                _bannerController.SetBannerText("Red Player's Turn");
            }

            // Reset everything
            _selectedTile = null;
            _selectedMoveCard = null;
            _selectedTileIndicator.SetActive(false);
            _selectedCardIndicator.SetActive(false);

            _gameState = GameState.WaitingForPlayerInput;
        });
    }

    // End the game
    public void EndGame(string winningColor)
    {
        _gameWonScreen.GameWon(winningColor);
        _gameState = GameState.GameWon;
    }

    
}
