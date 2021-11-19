using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Text _playerOneScoreText;
    [SerializeField] private static Text _playerOneScoreStat;
    [SerializeField] private static int _playerOneScoreCount=0;

    [SerializeField] private Text _playerTwoScorText;
    [SerializeField] private static Text _playerTwoScoreStat;
    [SerializeField] private static int _playerTwoScoreCount=0;

    void Awake()
    {
        _playerOneScoreStat = _playerOneScoreText;
        _playerOneScoreStat.text = "0";
        _playerTwoScoreStat = _playerTwoScorText;
        _playerTwoScoreStat.text = "0";
    }

    public static void AddScore(int player,int score)
    {
        switch (player)
        {
            case 1:
                _playerOneScoreCount += score;
                _playerOneScoreStat.text = _playerOneScoreCount.ToString();
                break;
            case 2:
                _playerTwoScoreCount += score;
                _playerTwoScoreStat.text = _playerTwoScoreCount.ToString();
                break;

        }
    }
}
