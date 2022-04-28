using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : Singleton<AIManager>
{
   public Vector2Int GenerateNewTurn(int sizeX,int sizeY)
    {
        return new Vector2Int(Random.Range(0, sizeX), Random.Range(0, sizeY));
    }
    
    public Vector2Int GenerateNewTurn(Vector2Int size)
    {
        return new Vector2Int(Random.Range(0, size.x), Random.Range(0, size.y));
    }
}
