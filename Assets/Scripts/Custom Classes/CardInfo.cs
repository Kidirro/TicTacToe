using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

[CreateAssetMenu(fileName = "New card", menuName = "Card")]
public class CardInfo : ScriptableObject
{
    public Sprite CardImageP1;
    public Sprite CardImageP2;
    public string CardName;
    public string CardDescription;

    [Space]
    [Space]
    [Space]

    public CardTypeImpact CardType;
    public CardBonusType CardBonus;
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

    public void PlaceFigureWithAddCard()
    {
        Vector4 CurrentArea = AreaManager.GetArea(Card.ChosedCell, CardAreaSize);
        for (int x = (int)CurrentArea.x; x <= CurrentArea.z; x++)
        {
            for (int y = (int)CurrentArea.y; y <= CurrentArea.w; y++)
            {
                TurnController.Instance.PlaceInCell(new Vector2Int(x,y));
            }
        }
        TurnController.Instance.MasterChecker((CellFigure)PlayerManager.Instance.GetCurrentPlayer().SideId);

        SlotManager.Instance.AddCard(PlayerManager.Instance.GetCurrentPlayer());
    }

    public void PlaceFigure()
    {
        Vector4 CurrentArea = AreaManager.GetArea(Card.ChosedCell, CardAreaSize);
        for (int x = (int)CurrentArea.x; x <= CurrentArea.z; x++)
        {
            for (int y = (int)CurrentArea.y; y <= CurrentArea.w; y++)
            {
                TurnController.Instance.PlaceInCell(new Vector2Int(x, y));
            }
        }
        TurnController.Instance.MasterChecker((CellFigure)PlayerManager.Instance.GetCurrentPlayer().SideId);
    }

    public void PlaceRandom5()
    {
        for (int i = 0; i <5; i++)
        {
            TurnController.Instance.PlaceInCell(AIManager.Instance.GenerateNewTurn(Field.Instance.FieldSize));
        }
        TurnController.Instance.MasterChecker((CellFigure)PlayerManager.Instance.GetCurrentPlayer().SideId);
    }  
    
    public void AddBonusMana_Effected()
    {
        Action f = delegate () { ManaManager.Instance.AddBonusMana(1); };
        Effect effect = new Effect(f, 1, PlayerManager.Instance.GetCurrentPlayer().SideId);;
        EffectManager.Instance.AddEffect(effect);
    }
    public void AddFigure_Effected()
    {
        Action f = delegate () { TurnController.Instance.PlaceInCell(AIManager.Instance.GenerateNewTurn(Field.Instance.FieldSize));
            TurnController.Instance.MasterChecker((CellFigure)PlayerManager.Instance.GetCurrentPlayer().SideId);
        };
        Effect effect = new Effect(f, 3, PlayerManager.Instance.GetCurrentPlayer().SideId); ;
        EffectManager.Instance.AddEffect(effect);

    }

}

public enum CardTypeImpact
{
    OnField,
    OnArea,
    OnAreaWithCheck
}

public enum CardBonusType
{
    None,
    Random,
    AddCard
}