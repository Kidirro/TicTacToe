using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : Singleton<AIManager>
{
    public BotGameType BotAggression;

   public Vector2Int GenerateNewTurn(int sizeX,int sizeY)
    {
        Vector2Int result = new Vector2Int(Random.Range(0, sizeX), Random.Range(0, sizeY));
        while (!Field.Instance.IsCellEmpty(result))
        {
            result = new Vector2Int(Random.Range(0, sizeX), Random.Range(0, sizeY));
        }
        return result;
    }
    
    public Vector2Int GenerateNewTurn(Vector2Int size)
    {
        Vector2Int result = new Vector2Int(Random.Range(0, size.x), Random.Range(0, size.y));
        while (!Field.Instance.IsCellEmpty(result))
        {
            result = new Vector2Int(Random.Range(0, size.x), Random.Range(0, size.y));
        }
        return result;
    }

    public int GetCellValue(Vector2Int currentId, int sideId)
    {
        int Ally_value = (BotAggression == BotGameType.Defense)? 2:1;
        int Enemy_value = (BotAggression == BotGameType.Attack)? 2:1;

        int resultValue = 0;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Cell nextCell = Field.Instance.GetNextCell(currentId, new Vector2Int(x, y));
                //Существует ли следующая клетка
                if (nextCell == null) continue;
                Debug.Log(nextCell.Id);
                Debug.Log(((sideId == (int)nextCell.Figure) ? Ally_value : Enemy_value));
                
                


                resultValue = resultValue + ((sideId == (int)nextCell.Figure) ? Ally_value : Enemy_value);

                //Если направление =0, то останавливаемся
                if (Mathf.Abs(nextCell.Id.x) + Mathf.Abs(nextCell.Id.y) == 0) continue;


                //Проверяем постследующую клетку
                Cell PostNextCell = Field.Instance.GetNextCell(nextCell.Id, new Vector2Int(x, y));


                //Существует ли постследующую клетка
                if (PostNextCell == null) continue;

                resultValue += (sideId == (int)nextCell.Figure) ? Ally_value : Enemy_value;
            }
        }

        return resultValue;
    }
}

public enum BotGameType
{
    Defense,
    Attack
}
