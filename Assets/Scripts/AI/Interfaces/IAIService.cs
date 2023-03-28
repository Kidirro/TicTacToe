using System;
using UnityEngine;

namespace AI.Interfaces
{
    public interface IAIService
    {
        public void StartBotTurn(int countFigure, int playerScore, int botScore, Action callback);
        public void StopBotTurnForce();
        public Vector2Int GenerateRandomPosition(Vector2Int size);
    }
}