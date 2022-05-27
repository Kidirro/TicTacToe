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
        TurnController.PlaceInCell(new Vector2Int(Card.ChosedCell.x, Card.ChosedCell.y));
    }

    public void PlaceRandom()
    {
        for (int i = 0; i < 3; i++)
        {
            TurnController.PlaceInCell(AIManager.Instance.GenerateNewTurn(Field.Instance.FieldSize));
        }
    }

}

public enum CardTypeImpact
{
    OnField,
    OnArea,
    OnAreaWithCheck
}