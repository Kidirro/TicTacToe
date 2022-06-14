using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    private Dictionary<int, int> _currentScoreList = new Dictionary<int, int>();

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
        _currentScoreList.Add(player,0);
    }

    public void ResetAllScore()
    {
        List<int> keyList = new List<int>(_currentScoreList.Keys);

        foreach (int key in keyList)
        {
            _currentScoreList[key] = 0;
        }
    }
}
