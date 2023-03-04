using System.Collections.Generic;
using Cards.CustomType;
using Cards.Interfaces;
using GameState;
using Managers;
using Network;
using Players.Interfaces;
using Zenject;

namespace Players
{

    public class PlayerManager : IPlayerService
    {
        private List<PlayerInfo> _players = new List<PlayerInfo>();
        private int _currentPlayer = 0;
        
        #region Interfaces

        private IHandPoolManipulator _handPoolManipulator;

        #endregion
        
        [Inject]
        private void Construct(IHandPoolManipulator handPoolManipulator)
        {
            _handPoolManipulator = handPoolManipulator;
        }

        public void AddPlayer(PlayerType type)
        {
            PlayerInfo player = new PlayerInfo
            {
                EntityType = type,
                SideId = _players.Count,
                FullDeckPool = _handPoolManipulator.CreateCardPull()
            };

            player.DeckPool = player.FullDeckPool;
            player.HandPool = new List<CardModel>();
            _players.Add(player);

        }
        
        public void AddPlayer(PlayerType type, int side)
        {
            PlayerInfo player = new PlayerInfo
            {
                EntityType = type,
                SideId = side,
                FullDeckPool = _handPoolManipulator.CreateCardPull()
            };
            player.DeckPool = player.FullDeckPool;
            player.HandPool = new List<CardModel>();
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

        public int GetCurrentSideOnDevice()
        {
            int result = (GameplayManager.IsOnline) ? RoomManager.GetCurrentPlayerSide() :
                (GameplayManager.TypeGame == GameplayManager.GameType.SingleHuman) ? GetCurrentPlayer().SideId : 1;
            
            return result;
        }      
        
        public PlayerInfo GetCurrentPlayerOnDevice()
        {
            PlayerInfo result = (GameplayManager.IsOnline) ? _players[RoomManager.GetCurrentPlayerSide()-1] :
                (GameplayManager.TypeGame == GameplayManager.GameType.SingleHuman) ? GetCurrentPlayer() : _players[0];
            
            return result;
        }

        public List<PlayerInfo> GetPlayers()
        {
            return _players;
        }
    }
}