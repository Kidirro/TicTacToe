using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Managers
{

    public class PlayerManager : Singleton<PlayerManager>
    {
        private List<PlayerInfo> _players = new List<PlayerInfo>();

        public List<PlayerInfo> Players
        {
            get { return _players; }
        }

        private int _currentPlayer = 0;

        public void AddPlayer(PlayerType type)
        {
            PlayerInfo player = new PlayerInfo();
            _players.Add(player);

            player.EntityType = type;
            player.SideId = _players.Count;
            player.FullDeckPool = CardManager.Instance.CreateDeck();
            player.DeckPool = player.FullDeckPool;
            player.HandPool = new List<Card>();

        }
        public void AddPlayer(PlayerType type, int side)
        {
            PlayerInfo player = new PlayerInfo();
            player.EntityType = type;
            player.SideId = side;
            player.FullDeckPool = CardManager.Instance.CreateDeck();
            player.DeckPool = player.FullDeckPool;
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

        public PlayerInfo GetNextPlayer()
        {
            int _nextPlayer = _currentPlayer + 1;
            if (_nextPlayer == _players.Count) _nextPlayer = 0;
            return _players[_nextPlayer];
        }

        public void ResetCurrentPlayer()
        {
            _currentPlayer = 0;
        }
    }
}