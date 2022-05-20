using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    private List<PlayerInfo> _players = new List<PlayerInfo>();

    private int _currentPlayer=0;

    public void AddPlayer(PlayerType type)
    {
        PlayerInfo player = new PlayerInfo();
        _players.Add(player);

        player.EntityType = type;
        player.SideId = _players.Count;
        player.FullDeckPool = new List<Card>(CardManager.Instance.CardAvaible);
        player.DeckPool = new List<Card>(CardManager.Instance.CardAvaible);
        player.HandPool = new List<Card>();

    }
    public void AddPlayer(PlayerType type, int side)
    {
        PlayerInfo player = new PlayerInfo();
        player.EntityType = type;
        player.SideId = side;
        player.FullDeckPool = new List<Card>(CardManager.Instance.CardAvaible);
        player.DeckPool = new List<Card>(CardManager.Instance.CardAvaible);
        player.HandPool = new List<Card>();
        _players.Add(player);
    }


    public PlayerInfo GetCurrentPlayer()
    {
        try
        {
            return _players[_currentPlayer];
        }
        catch
        {
            return null;
        }
    }

    public void NextPlayer()
    {
        _currentPlayer += 1;
        if (_currentPlayer == _players.Count) _currentPlayer = 0;
    }

}
