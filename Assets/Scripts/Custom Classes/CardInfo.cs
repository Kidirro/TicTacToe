using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using Managers;
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
    public int CardBonusManacost = 0;
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
                Field.Instance.PlaceInCell(new Vector2Int(x, y));
            }
        }

          FinishLineManager.Instance.MasterChecker((CellFigure)PlayerManager.Instance.GetCurrentPlayer().SideId);

        SlotManager.Instance.AddCard(PlayerManager.Instance.GetCurrentPlayer());
    }

    public void PlaceFigure()
    {
        Vector4 CurrentArea = AreaManager.GetArea(Card.ChosedCell, CardAreaSize);
        for (int x = (int)CurrentArea.x; x <= CurrentArea.z; x++)
        {
            for (int y = (int)CurrentArea.y; y <= CurrentArea.w; y++)
            {
                Field.Instance.PlaceInCell(new Vector2Int(x, y));
                FinishLineManager.Instance.MasterChecker((CellFigure)PlayerManager.Instance.GetCurrentPlayer().SideId);
            }
        }
    }

    public void PlaceRandom5()
    {
        for (int i = 0; i < 5; i++)
        {
            Vector2Int position = AIManager.Instance.GenerateRandomPosition(Field.Instance.FieldSize);
            if (position == new Vector2Int(-1, -1)) continue;
            Field.Instance.PlaceInCell(position);
            FinishLineManager.Instance.MasterChecker((CellFigure)PlayerManager.Instance.GetCurrentPlayer().SideId);
        }
    }

    public void AddBonusMana_Effected()
    {
        Action f = delegate ()
        {
            ManaManager.Instance.AddBonusMana(1);
            ManaManager.Instance.RestoreAllMana();
            ManaManager.Instance.UpdateManaUI();
        };
        Effect effect = new Effect(f, 1, PlayerManager.Instance.GetCurrentPlayer().SideId, Effect.EffectTypes.Parallel,0); 
        EffectManager.Instance.AddEffect(effect);
    }

    public void AddFigure_Effected()
    {
        Action f = delegate ()
        {
            Vector2Int position = AIManager.Instance.GenerateRandomPosition(Field.Instance.FieldSize);
            if (position == new Vector2Int(-1, -1)) return;
            
                Field.Instance.PlaceInCell(position);               
            
        };
        Effect effect = new Effect(f, 3, PlayerManager.Instance.GetCurrentPlayer().SideId,Effect.EffectTypes.Consistently,1);
        EffectManager.Instance.AddEffect(effect);

    }

    public void Decrease2MaxMana()
    {
        ManaManager.Instance.IncreaseMaxMana(-2);
        ManaManager.Instance.UpdateManaUI();
        Action d = delegate ()
        {
            ManaManager.Instance.IncreaseMaxMana(2);
            ManaManager.Instance.RestoreAllMana();
            ManaManager.Instance.UpdateManaUI();
        };
        Effect effect = new Effect(delegate () { }, 3, PlayerManager.Instance.GetCurrentPlayer().SideId,Effect.EffectTypes.Parallel,0, d);
        EffectManager.Instance.AddEffect(effect);

    }

    public void Increase2MaxMana()
    {
        ManaManager.Instance.IncreaseMaxMana(2);
        ManaManager.Instance.UpdateManaUI();
        Action d = delegate ()
        {
            ManaManager.Instance.IncreaseMaxMana(-2);
            ManaManager.Instance.RestoreAllMana();
            ManaManager.Instance.UpdateManaUI();
        };
        Effect effect = new Effect(delegate () { }, 3, PlayerManager.Instance.GetCurrentPlayer().SideId,Effect.EffectTypes.Parallel,0, d);
        EffectManager.Instance.AddEffect(effect);

    }

    public void Random2Mana()
    {
        Action f = delegate ()
        {
            ManaManager.Instance.AddBonusMana(Random.Range(0, 2) == 0 ? -2 : 2);
            ManaManager.Instance.RestoreAllMana();
            ManaManager.Instance.UpdateManaUI();
        };
        Effect effect = new Effect(f, 1, PlayerManager.Instance.GetCurrentPlayer().SideId,Effect.EffectTypes.Parallel,0);
        EffectManager.Instance.AddEffect(effect);

    }
    public void DecreaseIncrease2Mana()
    {
        Action f = delegate ()
        {
            ManaManager.Instance.AddBonusMana(-2);
            ManaManager.Instance.RestoreAllMana();
            ManaManager.Instance.UpdateManaUI();
        };
        Action d = delegate ()
        {


            ManaManager.Instance.AddBonusMana(4);
            ManaManager.Instance.RestoreAllMana();
            ManaManager.Instance.UpdateManaUI();
        };


        Effect effect = new Effect(f, 2, PlayerManager.Instance.GetCurrentPlayer().SideId,Effect.EffectTypes.Parallel,0, d);
        EffectManager.Instance.AddEffect(effect);

    }

    public void FreezeCell()
    {
        Field.Instance.FreezeCell(Card.ChosedCell, (PlayerManager.Instance.GetCurrentPlayer().SideId == 1) ? CardHighlightP1 : CardHighlightP2, CardAreaSize);
        FinishLineManager.Instance.MasterChecker((CellFigure)PlayerManager.Instance.GetCurrentPlayer().SideId);
    }


    public void FreezeCellGroup()
    {
        List<Cell> _posList = new List<Cell>();
        for (int i = 0; i < 3; i++)
        {
            Vector2Int result = AIManager.Instance.GenerateRandomPosition(Field.Instance.FieldSize);
            if (result == new Vector2Int(-1, -1)) continue;
            
                while (_posList.IndexOf(Field.Instance.CellList[result.x][result.y]) != -1) result = AIManager.Instance.GenerateRandomPosition(Field.Instance.FieldSize);

                Field.Instance.FreezeCell(result, (PlayerManager.Instance.GetCurrentPlayer().SideId == 1) ? CardHighlightP1 : CardHighlightP2, CardAreaSize);
                _posList.Add(Field.Instance.CellList[result.x][result.y]);
            
        }
       FinishLineManager.Instance.MasterChecker((CellFigure)PlayerManager.Instance.GetCurrentPlayer().SideId);
    }


    public void Freeze3Cell_Effected()
    {
        Sprite sprite = (PlayerManager.Instance.GetCurrentPlayer().SideId == 1) ? CardHighlightP1 : CardHighlightP2;
        Action f = delegate ()
        {
            Vector2Int position = AIManager.Instance.GenerateRandomPosition(Field.Instance.FieldSize);
            if (position == new Vector2Int(-1, -1)) return;

            Field.Instance.FreezeCell(position, (PlayerManager.Instance.GetCurrentPlayer().SideId == 1) ? CardHighlightP1 : CardHighlightP2, CardAreaSize);            
        };

        Effect effect = new Effect(f, 3, PlayerManager.Instance.GetCurrentPlayer().SideId,Effect.EffectTypes.Parallel,3,null,Cell.AnimationTime);
        EffectManager.Instance.AddEffect(effect);
    }

    public void Restore1Mana()
    {
        ManaManager.Instance.RestoreMana(1);
        ManaManager.Instance.UpdateManaUI();
    }

    public void RestoreAllMana()
    {
        ManaManager.Instance.RestoreAllMana();
        ManaManager.Instance.UpdateManaUI();
    }

    public void Prikol()
    {
        int i = 0;
        Action f = delegate ()
        {
            i++;
            Debug.LogFormat("Action Up: {0}", i);
        };
        Action d = delegate ()
        {
            i++;
            Debug.LogFormat("Action dis: {0}", i);
        };
        //Effect effect = new Effect(f, 3, PlayerManager.Instance.GetCurrentPlayer().SideId, d);
        //EffectManager.Instance.AddEffect(effect);
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
