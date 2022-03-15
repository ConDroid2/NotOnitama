using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCardViewer : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private MoveCardController _moveCardController;
    [SerializeField] private MoveCardDatabase _database;

    private int _currentViewIndex;

    private void Awake()
    {
        _currentViewIndex = 0;
        _moveCardController.Initialize();
        _moveCardController.SetMoveCard(_database.MoveCards[_currentViewIndex], false);     
    }

    private void Update()
    {
        // Shift left or right through the cards
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(_currentViewIndex > 0)
            {
                _currentViewIndex--;
                _moveCardController.SetMoveCard(_database.MoveCards[_currentViewIndex], false);
            }
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (_currentViewIndex < _database.MoveCards.Count - 1)
            {
                _currentViewIndex++;
                _moveCardController.SetMoveCard(_database.MoveCards[_currentViewIndex], false);
            }
        }
    }
}
