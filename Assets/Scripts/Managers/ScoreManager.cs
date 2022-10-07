using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Managers
{

    public class ScoreManager : Singleton<ScoreManager>
    {
        private Dictionary<int, int> _currentScoreList = new Dictionary<int, int>();

        public static int _scoreForWin = 100;

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

        public void ResetAllScore()
        {
            List<int> keyList = new List<int>(_currentScoreList.Keys);

            foreach (int key in keyList)
            {
                _currentScoreList[key] = 0;
            }
        }

        public bool IsExistWinner()
        {
            foreach (int res in _currentScoreList.Keys)
            {
                if (_currentScoreList[res] >= _scoreForWin) return true;
            }
            return false;
        }

        public int GetWinner()
        {
            int maxScore = -1;
            int maxKey = -1;
            foreach (int res in _currentScoreList.Keys)
            {
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
    }
}