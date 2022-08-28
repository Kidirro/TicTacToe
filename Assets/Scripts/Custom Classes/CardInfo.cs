using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "New card", menuName = "Card")]
public class CardInfo : ScriptableObject
{
    [Space, Header("Images")]
    public Sprite CardImageP1;
    public Sprite CardImageP2;
    public Sprite CardHighlightP1;
    public Sprite CardHighlightP2;
    public Color CardHighlightColor;

    [Space, Header("Name")]
    public string CardName;
    public string CardDescription;

    [Space]
    [Space]
    [Space]

    public bool IsDefaultUnlock;
    public CardTypeImpact CardType;
    public CardBonusType CardBonus;
    public Vector2Int CardAreaSize;
    public int CardCount;

    public bool IsNeedShowTip;
    public string TipText;

    [Range(0, 5)]
    public int CardManacost;
    [HideInInspector]
    public int CardBonusManacost=0;
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
                FieldCellLineManager.Instance.PlaceInCell(new Vector2Int(x,y));
            }
        }
        FieldCellLineManager.Instance.MasterChecker((CellFigure)PlayerManager.Instance.GetCurrentPlayer().SideId);

        SlotManager.Instance.AddCard(PlayerManager.Instance.GetCurrentPlayer());
    }

    public void PlaceFigure()
    {
        Vector4 CurrentArea = AreaManager.GetArea(Card.ChosedCell, CardAreaSize);
        for (int x = (int)CurrentArea.x; x <= CurrentArea.z; x++)
        {
            for (int y = (int)CurrentArea.y; y <= CurrentArea.w; y++)
            {
                FieldCellLineManager.Instance.PlaceInCell(new Vector2Int(x, y));
            }
        }
        FieldCellLineManager.Instance.MasterChecker((CellFigure)PlayerManager.Instance.GetCurrentPlayer().SideId);
    }

    public void PlaceRandom5()
    {
        for (int i = 0; i <5; i++)
        {
            FieldCellLineManager.Instance.PlaceInCell(AIManager.Instance.GenerateRandomPosition(Field.Instance.FieldSize));
        }
        FieldCellLineManager.Instance.MasterChecker((CellFigure)PlayerManager.Instance.GetCurrentPlayer().SideId);
    }  
    
    public void AddBonusMana_Effected()
    {
        Action f = delegate () { ManaManager.Instance.AddBonusMana(1); };
        Effect effect = new Effect(f, 1, PlayerManager.Instance.GetCurrentPlayer().SideId);;
        EffectManager.Instance.AddEffect(effect);
    }

    public void AddFigure_Effected()
    {
        Action f = delegate () { FieldCellLineManager.Instance.PlaceInCell(AIManager.Instance.GenerateRandomPosition(Field.Instance.FieldSize));
            FieldCellLineManager.Instance.MasterChecker((CellFigure)PlayerManager.Instance.GetCurrentPlayer().SideId);
        };
        Effect effect = new Effect(f, 3, PlayerManager.Instance.GetCurrentPlayer().SideId);
        EffectManager.Instance.AddEffect(effect);

    }

    public void FreezeCell_Efected()
    {
        Cell posCard = Field.Instance.CellList[Card.ChosedCell.x][Card.ChosedCell.y];

        Sprite sprite = (PlayerManager.Instance.GetCurrentPlayer().SideId == 1) ? CardHighlightP1 : CardHighlightP2;
        Action f = delegate () {
            Field.Instance.SetSubStateZone(posCard.Id,
                                            CardAreaSize,
                                            sprite,
                                            Color.black,
                                            CellSubState.block
                                          );
        };
        Action d = delegate () {

            Field.Instance.ResetSubStateZone(posCard.Id,
                                            CardAreaSize
                                          );
        };
        f.Invoke();
        Effect effect = new Effect(f, 2, PlayerManager.Instance.GetCurrentPlayer().SideId, d);
        EffectManager.Instance.AddEffect(effect);
        FieldCellLineManager.Instance.MasterChecker((CellFigure)PlayerManager.Instance.GetCurrentPlayer().SideId);
    }
    
    
    public void FreezeCellGroup_Efected()
    {
        List<Cell> _posList = new List<Cell>();
        for (int i = 0; i < 3; i++)
        {
            Vector2Int result = new Vector2Int(Random.Range(0, Field.Instance.FieldSize.x), Random.Range(0, Field.Instance.FieldSize.y));
            while(_posList.IndexOf(Field.Instance.CellList[result.x][result.y])!=-1|| !Field.Instance.IsCellEnableToPlace(result)) result = new Vector2Int(Random.Range(0, Field.Instance.FieldSize.x), Random.Range(0, Field.Instance.FieldSize.y));
            _posList.Add(Field.Instance.CellList[result.x][result.y]);
        }
        Sprite sprite = (PlayerManager.Instance.GetCurrentPlayer().SideId == 1) ? CardHighlightP1 : CardHighlightP2;
        Action f = delegate () {

            foreach (Cell pos in _posList)
            {
                Field.Instance.SetSubStateZone(pos.Id,
                                                CardAreaSize,
                                                sprite,
                                                Color.black,
                                                CellSubState.block
                                              );
            }
        };
        Action d = delegate () {
            foreach (Cell pos in _posList)
            {
                Field.Instance.ResetSubStateZone(pos.Id,
                                            CardAreaSize
                                          );
            }
        };
        f.Invoke();
        Effect effect = new Effect(f, 2, PlayerManager.Instance.GetCurrentPlayer().SideId, d);
        EffectManager.Instance.AddEffect(effect);
        FieldCellLineManager.Instance.MasterChecker((CellFigure)PlayerManager.Instance.GetCurrentPlayer().SideId);
    }

    public void Prikol()
    {
        int i = 0;
        Action f = delegate () {
            i++;
            Debug.LogFormat("Action Up: {0}",i);
        };        
        Action d= delegate () {
            i++;
            Debug.LogFormat("Action dis: {0}", i);
        };
        Effect effect = new Effect(f, 3, PlayerManager.Instance.GetCurrentPlayer().SideId,d); 
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