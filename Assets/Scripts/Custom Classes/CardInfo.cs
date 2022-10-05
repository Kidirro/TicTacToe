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
    [HideInInspector]
    public int CardId;

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
                Vector2Int position = new Vector2Int(x, y);
                Field.Instance.PlaceInCell(position);
                NetworkEventManager.RaiseEventPlaceInCell(position);
            }
        }
        NetworkEventManager.RaiseEventMasterChecker();
        FinishLineManager.Instance.MasterChecker(PlayerManager.Instance.GetCurrentPlayer().SideId);

        SlotManager.Instance.AddCard(PlayerManager.Instance.GetCurrentPlayer());
    }

    public void PlaceFigure()
    {
        Vector4 CurrentArea = AreaManager.GetArea(Card.ChosedCell, CardAreaSize);
        for (int x = (int)CurrentArea.x; x <= CurrentArea.z; x++)
        {
            for (int y = (int)CurrentArea.y; y <= CurrentArea.w; y++)
            {
                Vector2Int position = new Vector2Int(x, y);

                Field.Instance.PlaceInCell(position);
                NetworkEventManager.RaiseEventPlaceInCell(position);

                FinishLineManager.Instance.MasterChecker(PlayerManager.Instance.GetCurrentPlayer().SideId);
                NetworkEventManager.RaiseEventMasterChecker();
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
            NetworkEventManager.RaiseEventPlaceInCell(position);

            FinishLineManager.Instance.MasterChecker(PlayerManager.Instance.GetCurrentPlayer().SideId);
            NetworkEventManager.RaiseEventMasterChecker();
        }
    }

    public void AddFigure_Effected()
    {
        EffectManager.Instance.AddFigure_Effect();
/**/
    }  
    
    #region Mana

    public void AddBonusMana_Effected()
    {
        EffectManager.Instance.AddBonusMana_Effect();

      /**/
    }

    public void Decrease2MaxMana()
    {
        ManaManager.Instance.IncreaseMaxMana(-2);
        NetworkEventManager.RaiseEventIncreaseMaxMana(-2);
        ManaManager.Instance.UpdateManaUI();
        EffectManager.Instance.Decrease2MaxMana_Effect();   
/**/
    }

    public void Increase2MaxMana()
    {
        ManaManager.Instance.IncreaseMaxMana(2);
        NetworkEventManager.RaiseEventIncreaseMaxMana(2);

        ManaManager.Instance.UpdateManaUI();
        EffectManager.Instance.Increase2MaxMana_Effect();
/**/

    }

    public void Random2Mana()
    {
        EffectManager.Instance.Random2Mana_Effect();
        /**/
    }

    public void DecreaseIncrease2Mana()
    {
        EffectManager.Instance.DecreaseIncrease2Mana_Effect();
/**/
    }

    #endregion

    #region Freeze

    public void FreezeCell()
    {
        Field.Instance.FreezeCell(Card.ChosedCell, (PlayerManager.Instance.GetCurrentPlayer().SideId == 1) ? CardHighlightP1 : CardHighlightP2);
        NetworkEventManager.RaiseEventSetSubState(Card.ChosedCell, CardId);
        FinishLineManager.Instance.MasterChecker(PlayerManager.Instance.GetCurrentPlayer().SideId);
    }


    public void FreezeCellGroup()
    {
        List<Cell> _posList = new List<Cell>();
        for (int i = 0; i < 3; i++)
        {
            Vector2Int result = AIManager.Instance.GenerateRandomPosition(Field.Instance.FieldSize);
            if (result == new Vector2Int(-1, -1)) continue;

            while (_posList.IndexOf(Field.Instance.CellList[result.x][result.y]) != -1) result = AIManager.Instance.GenerateRandomPosition(Field.Instance.FieldSize);

            Field.Instance.FreezeCell(result, (PlayerManager.Instance.GetCurrentPlayer().SideId == 1) ? CardHighlightP1 : CardHighlightP2);
            NetworkEventManager.RaiseEventSetSubState(result, CardId);
            _posList.Add(Field.Instance.CellList[result.x][result.y]);

        }
        FinishLineManager.Instance.MasterChecker(PlayerManager.Instance.GetCurrentPlayer().SideId);
        NetworkEventManager.RaiseEventMasterChecker();
    }


    public void Freeze3Cell_Effected()
    {
        Sprite sprite = (PlayerManager.Instance.GetCurrentPlayer().SideId == 1) ? CardHighlightP1 : CardHighlightP2;
        Action f = delegate ()
        {
            Vector2Int position = AIManager.Instance.GenerateRandomPosition(Field.Instance.FieldSize);
            if (position == new Vector2Int(-1, -1)) return;

            Field.Instance.FreezeCell(position, (PlayerManager.Instance.GetCurrentPlayer().SideId == 1) ? CardHighlightP1 : CardHighlightP2);
            NetworkEventManager.RaiseEventSetSubState(position, CardId);
        };

        Effect effect = new Effect(f, 3, PlayerManager.Instance.GetCurrentPlayer().SideId, Effect.EffectTypes.Parallel, 3, null, Cell.AnimationTime);
        EffectManager.Instance.AddEffect(effect);
    }

    public void FreezeFigure()
    {
        List<Cell> AllCells = Field.Instance.GetAllCellWithFigure((CellFigure)PlayerManager.Instance.GetCurrentPlayer().SideId);
        List<Cell> resultCells = new List<Cell>();
        while (AllCells.Count > 0 && resultCells.Count < 5)
        {
            int randValue = Random.Range(0, AllCells.Count);
            resultCells.Add(AllCells[randValue]);
            AllCells.Remove(AllCells[randValue]);
        }

        Sprite sprite = (PlayerManager.Instance.GetCurrentPlayer().SideId == 1) ? CardHighlightP1 : CardHighlightP2;
        for (int i = 0; i < resultCells.Count; i++)
        {
            Field.Instance.FreezeCell(resultCells[i].Id, sprite);
            NetworkEventManager.RaiseEventSetSubState(resultCells[i].Id, CardId);
        }

    }

    public void BreakIce()
    {
        List<Effect> effects = EffectManager.Instance.EffectList.FindAll(x => x.EffectPriority == 2);
        List<Effect> resultEffects = new List<Effect>();
        while (effects.Count > 0 && resultEffects.Count < 6)
        {
            int randValue = Random.Range(0, effects.Count);
            resultEffects.Add(effects[randValue]);
            effects.Remove(effects[randValue]);
        }
        if (resultEffects.Count == 0) return;
        for (int i = 0; i < resultEffects.Count; i++)
        {
            resultEffects[i].OnEffectDisable.Invoke();
            EffectManager.Instance.EffectList.Remove(resultEffects[i]);
        }
        CoroutineManager.Instance.AddAwaitTime(resultEffects[0].EffectTimeDisable);
        FinishLineManager.Instance.MasterChecker(PlayerManager.Instance.GetCurrentPlayer().SideId);

    }

    public void FreezeAllMana()
    {
        while (ManaManager.Instance.IsEnoughMana(1))
        {
            ManaManager.Instance.IncreaseMana(-1);
            NetworkEventManager.RaiseEventIncreaseMana(-1);
            for (int i =0; i<2; i++)
            {
                Vector2Int result = AIManager.Instance.GenerateRandomPosition(Field.Instance.FieldSize);
                if (result == new Vector2Int(-1, -1)) break;
                Field.Instance.FreezeCell(result, (PlayerManager.Instance.GetCurrentPlayer().SideId == 1) ? CardHighlightP1 : CardHighlightP2);
            }

        }
        ManaManager.Instance.UpdateManaUI();
        CoroutineManager.Instance.AddAwaitTime(Cell.AnimationTime);        
        FinishLineManager.Instance.MasterChecker(PlayerManager.Instance.GetCurrentPlayer().SideId);

    }

    public void ManaPerIce()
    {
        List<Effect> effects = EffectManager.Instance.EffectList.FindAll(x => x.EffectPriority == 2);
        int resultMana = effects.Count / 3;
        ManaManager.Instance.IncreaseMana(resultMana, true);
        NetworkEventManager.RaiseEventIncreaseMana(resultMana,true);
        ManaManager.Instance.UpdateManaUI();
    }

    #endregion

    public void Restore1Mana()
    {
        ManaManager.Instance.RestoreMana(1);
        NetworkEventManager.RaiseEventRestoreMana(1);
        ManaManager.Instance.UpdateManaUI();
    }

    public void RestoreAllMana()
    {
        ManaManager.Instance.RestoreAllMana();
        NetworkEventManager.RaiseEventRestoreMana(-1);
        ManaManager.Instance.UpdateManaUI();
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
