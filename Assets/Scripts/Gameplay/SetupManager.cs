using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupManager : MonoBehaviour
{
    [Header("Manager References")]
    public GridManager GridManager;
    public MoveCardDatabase CardDatabase;


    [Header("Piece Prefabs")]
    [SerializeField] private Piece _redStudentPrefab;
    [SerializeField] private Piece _blueStudentPrefab;
    [SerializeField] private Piece _redMasterPrefab;
    [SerializeField] private Piece _blueMasterPrefab;

    public void Initialize(GameManager gameManager)
    {
        // Set up the grid
        SetupGrid(gameManager);

        // Set up Players
        SetUpPlayers(gameManager.BluePlayer, gameManager.RedPlayer);

        // Setup Cards
        SetUpMoveCards(gameManager);
    }

    // Sets up the grid, tells game manager to listen to events
    private void SetupGrid(GameManager gameManager)
    {
        GridManager.SetupGrid();

        foreach(Tile tile in GridManager.TileDictionary.Values)
        {
            tile.OnPieceDestroyed += gameManager.HandlePieceDestroyed;
            tile.OnPieceMovedToTile += gameManager.HandlePieceMovedToTile;
        }
    }

    // Create the pieces and assign them to the right player
    private void SetUpPlayers(Player bluePlayer, Player redPlayer)
    {
        // Spawn the pieces       
        Piece blueMaster;
        blueMaster = Instantiate(_blueMasterPrefab);
        bluePlayer.Pieces.Add(blueMaster);

        Piece redMaster;
        redMaster = Instantiate(_redMasterPrefab);
        redPlayer.Pieces.Add(redMaster);

        for (int i = 0; i < 4; i++)
        {
            Piece newBlueStudent = Instantiate(_blueStudentPrefab);
            bluePlayer.Pieces.Add(newBlueStudent);

            Piece newRedStudent = Instantiate(_redStudentPrefab);
            redPlayer.Pieces.Add(newRedStudent);       
        }

        // Put the masters in the right spot (first index is master)
        GridManager.SetPieceToTile(2, 0, bluePlayer.Pieces[0]);
        GridManager.SetPieceToTile(2, 4, redPlayer.Pieces[0]);

        // Put the students in the right spot
        GridManager.SetPieceToTile(0, 0, bluePlayer.Pieces[1]);
        GridManager.SetPieceToTile(1, 0, bluePlayer.Pieces[2]);
        GridManager.SetPieceToTile(3, 0, bluePlayer.Pieces[3]);
        GridManager.SetPieceToTile(4, 0, bluePlayer.Pieces[4]);

        GridManager.SetPieceToTile(0, 4, redPlayer.Pieces[1]);
        GridManager.SetPieceToTile(1, 4, redPlayer.Pieces[2]);
        GridManager.SetPieceToTile(3, 4, redPlayer.Pieces[3]);
        GridManager.SetPieceToTile(4, 4, redPlayer.Pieces[4]);
    }

    // Pick the 5 move cards to use for the game, assign them to the cards
    public void SetUpMoveCards(GameManager gameManager)
    {
        if (gameManager.MoveCards.Count != 5)
        {
            Debug.Log("The game has not been set up correctly, there are not 5 move cards");
        }

        List<MoveCard> moveCardsData = new List<MoveCard>();

        // Keep picking move cards till we one for each Move Card (5)
        while(moveCardsData.Count < 5)
        {
            // Get a random int
            int randomIndex = Random.Range(0, CardDatabase.MoveCards.Count);

            // Get the associated move card
            MoveCard moveCard = CardDatabase.MoveCards[randomIndex];

            // If we haven't picked it yet, add it to the list
            if(moveCardsData.Contains(moveCard) == false)
            {
                moveCardsData.Add(moveCard);
            }
        }

        // Initialize all the move cards
        for(int i = 0; i < 5; i++)
        {
            gameManager.MoveCards[i].Initialize();
            gameManager.MoveCards[i].SetMoveCard(moveCardsData[i], false);
        }

        // Assign cards to players and put them in the right spot
        gameManager.BluePlayer.LeftCard = gameManager.MoveCards[0];
        gameManager.BluePlayer.LeftCard.transform.position = gameManager.BluePlayerLeftCardPos.position;

        gameManager.BluePlayer.RightCard = gameManager.MoveCards[1];
        gameManager.BluePlayerRightCardPos.transform.position = gameManager.BluePlayerRightCardPos.position;

        gameManager.RedPlayer.LeftCard = gameManager.MoveCards[2];
        gameManager.RedPlayer.LeftCard.transform.position = gameManager.RedPlayerLeftCardPos.position;

        gameManager.RedPlayer.RightCard = gameManager.MoveCards[3];
        gameManager.RedPlayer.RightCard.transform.position = gameManager.RedPlayerRightCardPos.position;

        gameManager.CenterCard = gameManager.MoveCards[4];
        gameManager.CenterCard.transform.position = gameManager.CenterCardPos.position;

        // Red player's cards need their grids to be flipped
        gameManager.RedPlayer.LeftCard.FlipGrid();
        gameManager.RedPlayer.RightCard.FlipGrid();
    }
}
