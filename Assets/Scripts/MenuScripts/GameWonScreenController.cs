using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameWonScreenController : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private GameObject _gameWonScreen;
    [SerializeField] private TextMeshProUGUI _winScreenText;

    //Set the winning text, activate the win screen
    public void GameWon(string winningColor)
    {
        _winScreenText.text = winningColor + " Won!";

        _gameWonScreen.SetActive(true);
    }
}
