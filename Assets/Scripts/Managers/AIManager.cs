using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class AIManager : Singleton<AIManager>
    {
        public BotGameType BotAggression;

        public Vector2Int GenerateNewTurn(int SideId)
        {
            if (ScoreManager.Instance.GetScore(PlayerManager.Instance.GetCurrentPlayer().SideId) > ScoreManager.Instance.GetScore(PlayerManager.Instance.GetNextPlayer().SideId))
                BotAggression = BotGameType.Attack;
            else BotAggression = BotGameType.Defense;

            Vector2Int maxId = new Vector2Int(-1, -1);
            int maxValue = -1;

            for (int x = 0; x < Field.Instance.FieldSize.x; x++)
            {
                for (int y = 0; y < Field.Instance.FieldSize.y; y++)
                {
                    Vector2Int currentId = new Vector2Int(x, y);
                    if (!Field.Instance.IsCellEnableToPlace(currentId)) continue;

                    int currentValue = GetCellValue(currentId, SideId);
                    if (currentValue == maxValue)
                    {
                        int RandomId = Random.Range(0, 2);
                        maxId = (RandomId == 0) ? maxId : currentId;
                        continue;
                    }
                    if (currentValue > maxValue)
                    {
                        maxValue = currentValue;
                        maxId = currentId;
                    }

                }
            }
            Debug.LogFormat("Final Value: {0}. Final Id: {1}.", maxValue, maxId);

            return maxId;
        }

        public Vector2Int GenerateNewTurn(int SideId, BotGameType aggression)
        {
            BotAggression = aggression;

            Vector2Int maxId = new Vector2Int(-1, -1);
            int maxValue = -1;

            for (int x = 0; x < Field.Instance.FieldSize.x; x++)
            {
                for (int y = 0; y < Field.Instance.FieldSize.y; y++)
                {
                    Vector2Int currentId = new Vector2Int(x, y);
                    if (!Field.Instance.IsCellEnableToPlace(currentId)) continue;

                    int currentValue = GetCellValue(currentId, SideId);
                    if (currentValue == maxValue)
                    {
                        int RandomId = Random.Range(0, 2);
                        maxId = (RandomId == 0) ? maxId : currentId;
                        continue;
                    }
                    if (currentValue > maxValue)
                    {
                        maxValue = currentValue;
                        maxId = currentId;
                    }

                }
            }
            Debug.LogFormat("Final Value: {0}. Final Id: {1}.", maxValue, maxId);

            return maxId;
        }


        public Vector2Int GenerateRandomPosition(Vector2Int size)
        {
            Vector2Int result = new Vector2Int(Random.Range(0, size.x), Random.Range(0, size.y));
            while (!Field.Instance.IsCellEnableToPlace(result))
            {
                result = new Vector2Int(Random.Range(0, size.x), Random.Range(0, size.y));
            }
            return result;
        }

        public int GetCellValue(Vector2Int currentId, int sideId)
        {
            int Ally_value = (BotAggression == BotGameType.Defense) ? 3 : 2;
            int Enemy_value = (BotAggression == BotGameType.Attack) ? 3 : 2;

            int resultValue = 0;

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Cell nextCell = Field.Instance.GetNextCell(currentId, new Vector2Int(x, y));
                    //Существует ли следующая клетка
                    if (nextCell == null) continue;

                    resultValue = resultValue + ((sideId == (int)nextCell.Figure) ? Ally_value : Enemy_value);

                    //Если направление =0, то останавливаемся
                    if (Mathf.Abs(nextCell.Id.x) + Mathf.Abs(nextCell.Id.y) == 0) continue;


                    //Проверяем постследующую клетку
                    Cell PostNextCell = Field.Instance.GetNextCell(nextCell.Id, new Vector2Int(x, y));


                    //Существует ли постследующую клетка
                    if (PostNextCell == null) continue;
                    //соответсвует ли клетка 
                    if (PostNextCell.Figure != nextCell.Figure) continue;

                    resultValue += (sideId == (int)nextCell.Figure) ? Ally_value * 2 : Enemy_value * 2;
                }
            }

            return resultValue;
        }


        public enum BotGameType
        {
            Defense,
            Attack
        }
    }
}