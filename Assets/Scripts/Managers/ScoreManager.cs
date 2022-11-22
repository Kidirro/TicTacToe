using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Managers
{

    public class ScoreManager : Singleton<ScoreManager>
    {
        private Dictionary<int, int> _currentScoreList = new Dictionary<int, int>();

        private List<int> _roundWinner = new List<int>();

        private const int _scoreForWin = 20;
        private const int _roundForWin = 2;
        private const int _maxRounds = 3;

        public int GetScore(int player)
        {
            return _currentScoreList[player];
        }

        public void SetScore(int player, int value)
        {
            _currentScoreList[player] = value;
        }

        public void AddScore(int player, int value)
        {
            _currentScoreList[player] += value;
        }

        public void AddPlayer(int player)
        {
            _currentScoreList.Add(player, 0);
        }
 
        public void RemovePlayer (int player)
        {
            _currentScoreList.Remove(player);
        }

        public void ClearAllScore()
        {
            List<int> keyList = new List<int>(_currentScoreList.Keys);

            foreach (int key in keyList)
            {
                _currentScoreList[key] = 0;
            }
        }

        public bool IsExistRoundWinner()
        {
            foreach (int res in _currentScoreList.Keys)
            {
                if (_currentScoreList[res] >= _scoreForWin) return true;
            }
            return false;
        }

        public int GetRoundWinner()
        {
            int maxScore = -1;
            int maxKey = -1;
            foreach (int res in _currentScoreList.Keys)
            {
                Debug.Log($"res: {res}. score: {_currentScoreList[res]}");
                if (_currentScoreList[res] > maxScore)
                {
                    maxScore = _currentScoreList[res];
                    maxKey = res;
                }
                else if (_currentScoreList[res] == maxScore)
                {
                    maxKey = -1;
                }
            }
            return maxKey;
        }

        public bool IsExistGameWinner()
        {
            int p1Count = GetCountRoundWin(1);
            int p2Count = GetCountRoundWin(2);
            return (p1Count == _roundForWin || p2Count == _roundForWin || _roundWinner.Count == _maxRounds);
        }   
        
        public int GetGameWinner()
        {
            int p1Count = _roundWinner.FindAll(x => x == 1).Count;
            int p2Count = _roundWinner.FindAll(x => x == 2).Count;
            if (p1Count == _roundForWin) return 1;
            else if (p2Count == _roundForWin) return 2;
            else return -1;
        }

        public void AddRoundWinner(int Side)
        {
            _roundWinner.Add(Side);
        }

        public void ClearRoundWinners()
        {
            _roundWinner = new List<int>();
        }

        public int GetCountRoundWin(int side)
        {
            return _roundWinner.FindAll(x => x == side).Count;
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.W)) Debug.Log(GetRoundWinner());
        }

    }
}