using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New card", menuName = "Card")]
public class CardInfo : ScriptableObject
{
    public Sprite CardImage;
    public string CardName;
    public string CardDescription;

    [Space]
    [Space]
    [Space]

    public CardTypeImpact CardType;
    public Vector2Int CardAreaSize;

    [Range(0, 5)]
    public int CardManacost;
    public UnityEvent ÑardAction;



    public void AddLineUp()
    {
        Field.Instance.AddLineUp();
    }

    public void AddLineDown()
    {
        Field.Instance.AddLineDown();
    }

    public void AddLineLeft()
    {
        Field.Instance.AddLineLeft();
    }

    public void AddLineRight()
    {
        Field.Instance.AddLineRight();
    }

    public void PlaceFigure()
    {
        Vector4 CurrentArea = AreaManager.GetArea(Card.ChosedCell, CardAreaSize);
        for (int x = (int)CurrentArea.x; x <= CurrentArea.z; x++)
        {
            for (int y = (int)CurrentArea.y; y <= CurrentArea.w; y++)
            {
                TurnController.Instance.PlaceInCell(new Vector2Int(x,y));
            }
        }
        TurnController.Instance.MasterChecker((CellState)PlayerManager.Instance.GetCurrentPlayer().SideId);
    }

    public void PlaceRandom()
    {
        for (int i = 0; i < 10; i++)
        {
            TurnController.Instance.PlaceInCell(AIManager.Instance.GenerateNewTurn(Field.Instance.FieldSize));
        }
        TurnController.Instance.MasterChecker((CellState)PlayerManager.Instance.GetCurrentPlayer().SideId);
    }

}

public enum CardTypeImpact
{
    OnField,
    OnArea,
    OnAreaWithCheck
}