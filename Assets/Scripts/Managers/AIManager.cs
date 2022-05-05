using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : Singleton<AIManager>
{
   public Vector2Int GenerateNewTurn(int sizeX,int sizeY)
    {
        Vector2Int result = new Vector2Int(Random.Range(0, sizeX), Random.Range(0, sizeY));
        while (!TurnController.IsCellEmpty(result))
        {
            result = new Vector2Int(Random.Range(0, sizeX), Random.Range(0, sizeY));
        }
        return result;
    }
    
    public Vector2Int GenerateNewTurn(Vector2Int size)
    {
        Vector2Int result = new Vector2Int(Random.Range(0, size.x), Random.Range(0, size.y));
        while (!TurnController.IsCellEmpty(result))
        {
            result = new Vector2Int(Random.Range(0, size.x), Random.Range(0, size.y));
        }
        return result;
    }
}
