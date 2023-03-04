using System.Collections;
using AI.Interfaces;
using Cards;
using Coroutine;
using GameState;
using History.Interfaces;
using Managers;
using Players.Interfaces;
using Score;
using UnityEngine;
using Zenject;

namespace AI
{
    public class AIController : MonoBehaviour, IAIService
    {
        private BotGameType _botAggression;
        private CardInfo _botCardDefault;
        
        #region Dependency

        private IPlayerService _playerService;
        private IHistoryService _historyService;


        [Inject]
        private void Construct(IPlayerService playerService, IHistoryService historyService)
        {
            _playerService = playerService;
            _historyService = historyService;
        }

        #endregion
        
        private Vector2Int GenerateNewTurn(int sideId)
        {
            if (ScoreManager.Instance.GetScore(_playerService.GetCurrentPlayer().SideId) > ScoreManager.Instance.GetScore(_playerService.GetNextPlayer().SideId))
                _botAggression = BotGameType.Attack;
            else _botAggression = BotGameType.Defense;

            Vector2Int maxId = new Vector2Int(-1, -1);
            int maxValue = -1;

            for (int x = 0; x < Field.Instance.FieldSize.x; x++)
            {
                for (int y = 0; y < Field.Instance.FieldSize.y; y++)
                {
                    Vector2Int currentId = new Vector2Int(x, y);
                    if (!Field.Instance.IsCellEnableToPlace(currentId)) continue;

                    int currentValue = GetCellValue(currentId, sideId);
                    if (currentValue == maxValue)
                    {
                        int randomId = Random.Range(0, 2);
                        maxId = (randomId == 0) ? maxId : currentId;
                        continue;
                    }
                    if (currentValue > maxValue)
                    {
                        maxValue = currentValue;
                        maxId = currentId;
                    }

                }
            }

            return maxId;
        }
        
        public Vector2Int GenerateRandomPosition(Vector2Int size)
        {
            if (!Field.Instance.IsExistEmptyCell()) return new Vector2Int(-1, -1);
            Vector2Int result = new Vector2Int(Random.Range(0, size.x), Random.Range(0, size.y));
            while (!Field.Instance.IsCellEnableToPlace(result))
            {
                result = new Vector2Int(Random.Range(0, size.x), Random.Range(0, size.y));
            }
            return result;
        }

        private int GetCellValue(Vector2Int currentId, int sideId)
        {
            int allyValue = (_botAggression == BotGameType.Defense) ? 3 : 2;
            int enemyValue = (_botAggression == BotGameType.Attack) ? 3 : 2;

            int resultValue = 0;

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Cell nextCell = Field.Instance.GetNextCell(currentId, new Vector2Int(x, y));
                    //���������� �� ��������� ������
                    if (nextCell == null) continue;

                    resultValue = resultValue + ((sideId == (int)nextCell.Figure) ? allyValue : enemyValue);

                    //���� ����������� =0, �� ���������������
                    if (Mathf.Abs(nextCell.Id.x) + Mathf.Abs(nextCell.Id.y) == 0) continue;


                    //��������� ������������� ������
                    Cell postNextCell = Field.Instance.GetNextCell(nextCell.Id, new Vector2Int(x, y));


                    //���������� �� ������������� ������
                    if (postNextCell == null) continue;
                    //������������ �� ������ 
                    if (postNextCell.Figure != nextCell.Figure) continue;

                    resultValue += (sideId == (int)nextCell.Figure) ? allyValue * 2 : enemyValue * 2;
                }
            }

            return resultValue;
        }

        public void StartBotTurn(int countFigure)
        {
            StartCoroutine(IBotTurnProcess(countFigure));
        }

        private IEnumerator IBotTurnProcess(int countFigure)
        {
            for (int i = 0; i < countFigure; i++)
            {
                Vector2Int position = GenerateNewTurn(_playerService.GetCurrentPlayer().SideId);
                if (position != new Vector2Int(-1, -1))
                {
                    Field.Instance.PlaceInCell(position);

                    FinishLineManager.Instance.MasterChecker(_playerService.GetCurrentPlayer().SideId);
                    _historyService.AddHistoryCard(_playerService.GetCurrentPlayer(), _botCardDefault);
                    Debug.LogFormat("Current tactic : {0}. Is Empty: {1}", _botAggression, CoroutineQueueController.isQueueEmpty);
                    while (!CoroutineQueueController.isQueueEmpty) yield return null;
                }
                else
                {
                    yield break;
                }

            }
            GameplayManager.Instance.SetGamePlayStateQueue(GameplayManager.GameplayState.NewTurn);

        }

        public void StopBotTurnForce()
        {
            StopAllCoroutines();
        }

        public enum BotGameType
        {
            Defense,
            Attack
        }
    }
}