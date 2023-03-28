using System;
using System.Collections.Generic;
using AI.Interfaces;
using Area.Interfaces;
using Cards.CustomType;
using Cards.Enum;
using Cards.Interfaces;
using Coroutine.Interfaces;
using Effects;
using Effects.Interfaces;
using Field.Interfaces;
using FinishLine.Interfaces;
using History.Interfaces;
using Mana.Interfaces;
using Network.Interfaces;
using Players.Interfaces;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Cards
{
    public class CardActions : ICardActionService
    {
        private readonly Dictionary<string, Action<Vector2Int, CardInfo>> _serializableActions;

        #region Dependency

        private IAreaService _areaService;
        private IFinishLineService _finishLineService;
        private IPlayerService _playerService;
        private IHandPoolManipulator _handPoolManipulator;
        private ISerializableEffects _serializableEffects;
        private IFieldFigureService _fieldFigureService;
        private IEffectEventNetworkService _effectEventNetworkService;
        private IManaEventNetworkService _manaEventNetworkService;
        private IManaService _manaService;
        private IManaUIService _manaUIService;
        private IAIService _aiService;
        private IFieldService _fieldService;
        private IEffectService _effectService;
        private ICoroutineAwaitService _coroutineAwaitService;
        private ICheckEventNetworkService _checkEventNetworkService;
        private ICardEventNetworkService _cardEventNetworkService;
        private ICoroutineService _coroutineService;
        private IHandPoolView _handPoolView;
        private IFieldZoneService _fieldZoneService;
        private IHistoryService _historyService;

        [Inject]
        private void Construct(IAreaService areaService,
            IFinishLineService finishLineService,
            IPlayerService playerService,
            IHandPoolManipulator handPoolManipulator,
            ISerializableEffects serializableEffects,
            IFieldFigureService fieldFigureService,
            IEffectEventNetworkService effectEventNetworkService,
            IManaEventNetworkService manaEventNetworkService,
            IManaService manaService,
            IManaUIService manaUIService,
            IAIService aiService,
            IFieldService fieldService,
            IEffectService effectService,
            ICoroutineAwaitService coroutineAwaitService,
            ICheckEventNetworkService checkEventNetworkService,
            ICardEventNetworkService cardEventNetworkService,
            ICoroutineService coroutineService,
            IHandPoolView handPoolView,
            IFieldZoneService fieldZoneService,
            IHistoryService historyService
        )
        {
            _areaService = areaService;
            _finishLineService = finishLineService;
            _playerService = playerService;
            _handPoolManipulator = handPoolManipulator;
            _serializableEffects = serializableEffects;
            _fieldFigureService = fieldFigureService;
            _effectEventNetworkService = effectEventNetworkService;
            _manaEventNetworkService = manaEventNetworkService;
            _manaService = manaService;
            _manaUIService = manaUIService;
            _aiService = aiService;
            _fieldService = fieldService;
            _effectService = effectService;
            _coroutineAwaitService = coroutineAwaitService;
            _checkEventNetworkService = checkEventNetworkService;
            _coroutineService = coroutineService;
            _handPoolView = handPoolView;
            _fieldZoneService = fieldZoneService;
            _cardEventNetworkService = cardEventNetworkService;
            _historyService = historyService;
        }

        #endregion

        private void PlaceFigureWithAddCard(Vector2Int chosenCell, CardInfo info)
        {
            Vector4 currentArea = _areaService.GetArea(chosenCell, info.CardAreaSize);
            for (int x = (int) currentArea.x; x <= currentArea.z; x++)
            {
                for (int y = (int) currentArea.y; y <= currentArea.w; y++)
                {
                    Vector2Int position = new Vector2Int(x, y);
                    _fieldFigureService.PlaceInCell(position);
                }
            }

            _finishLineService.MasterChecker(_playerService.GetCurrentPlayer().SideId);
            _handPoolManipulator.AddCard(_playerService.GetCurrentPlayer());
        }

        public void PlaceFigure(Vector2Int chosenCell, CardInfo info)
        {
            Vector4 currentArea = _areaService.GetArea(chosenCell, info.CardAreaSize);
            for (int x = (int) currentArea.x; x <= currentArea.z; x++)
            {
                for (int y = (int) currentArea.y; y <= currentArea.w; y++)
                {
                    Vector2Int position = new Vector2Int(x, y);

                    _fieldFigureService.PlaceInCell(position);

                    _finishLineService.MasterChecker(_playerService.GetCurrentPlayer().SideId);
                }
            }
        }

        public void PlaceRandom5(Vector2Int chosenCell, CardInfo info)
        {
            for (int i = 0; i < 5; i++)
            {
                _fieldFigureService.PlaceInRandomCell();
                _finishLineService.MasterChecker(_playerService.GetCurrentPlayer().SideId);
            }
        }

        public void AddFigure_Effected(Vector2Int chosenCell, CardInfo info)
        {
            _serializableEffects.AddFigure_Effect();
            _effectEventNetworkService.RaiseEventAddEffect(_serializableEffects.AddFigure_Effect);
        }

        #region Mana

        public void AddBonusMana_Effected(Vector2Int chosenCell, CardInfo info)
        {
            _serializableEffects.AddBonusMana_Effect();
            _effectEventNetworkService.RaiseEventAddEffect(_serializableEffects.AddBonusMana_Effect);
        }

        public void Decrease2MaxMana(Vector2Int chosenCell, CardInfo info)
        {
            _manaService.IncreaseMaxMana(-2);
            if (_manaService.GetCurrentMana() > _manaService.GetMaxMana())
                _manaService.IncreaseMana(_manaService.GetMaxMana() - _manaService.GetCurrentMana());
            _manaEventNetworkService.RaiseEventIncreaseMaxMana(-2);
            _manaUIService.UpdateManaUI();

            _serializableEffects.Decrease2MaxMana_Effect();
            _effectEventNetworkService.RaiseEventAddEffect(_serializableEffects.Decrease2MaxMana_Effect);
        }

        public void Increase2MaxMana(Vector2Int chosenCell, CardInfo info)
        {
            _manaService.IncreaseMaxMana(2);
            _manaEventNetworkService.RaiseEventIncreaseMaxMana(2);

            _manaUIService.UpdateManaUI();
            _serializableEffects.Increase2MaxMana_Effect();
            _effectEventNetworkService.RaiseEventAddEffect(_serializableEffects.Increase2MaxMana_Effect);
        }

        public void Random2Mana(Vector2Int chosenCell, CardInfo info)
        {
            _serializableEffects.Random2Mana_Effect();
            _effectEventNetworkService.RaiseEventAddEffect(_serializableEffects.Random2Mana_Effect);
        }

        public void DecreaseIncrease2Mana(Vector2Int chosenCell, CardInfo info)
        {
            _serializableEffects.DecreaseIncrease2Mana_Effect();
            _effectEventNetworkService.RaiseEventAddEffect(_serializableEffects.DecreaseIncrease2Mana_Effect);
        }

        public void Restore1Mana(Vector2Int chosenCell, CardInfo info)
        {
            _manaService.RestoreMana(1);
            _manaEventNetworkService.RaiseEventRestoreMana(1);
            _manaUIService.UpdateManaUI();
        }

        public void RestoreAllMana(Vector2Int chosenCell, CardInfo info)
        {
            _manaService.RestoreAllMana();
            _manaEventNetworkService.RaiseEventRestoreMana(-1);
            _manaUIService.UpdateManaUI();
        }

        #endregion

        #region Freeze

        public void FreezeCell(Vector2Int chosenCell, CardInfo info)
        {
            _fieldFigureService.FreezeCell(chosenCell);


            _serializableEffects.FreezeCell_Effect(chosenCell);
            _effectEventNetworkService.RaiseEventAddFreezeEffect(chosenCell);


            _finishLineService.MasterChecker(_playerService.GetCurrentPlayer().SideId);
        }


        public void FreezeCellGroup(Vector2Int chosenCell, CardInfo info)
        {
            List<Cell> posList = new List<Cell>();
            for (int i = 0; i < 3; i++)
            {
                Vector2Int result = _aiService.GenerateRandomPosition(_fieldService.GetFieldSize());
                if (result == new Vector2Int(-1, -1)) continue;

                while (posList.IndexOf(_fieldService.GetCellLink(result)) != -1)
                    result = _aiService.GenerateRandomPosition(_fieldService.GetFieldSize());

                _fieldFigureService.FreezeCell(result);

                _serializableEffects.FreezeCell_Effect(result);
                _effectEventNetworkService.RaiseEventAddFreezeEffect(result);
                posList.Add(_fieldService.GetCellLink(result));
            }

            _finishLineService.MasterChecker(_playerService.GetCurrentPlayer().SideId);
        }


        public void Freeze3Cell_Effected(Vector2Int chosenCell, CardInfo info)
        {
            _serializableEffects.Freeze3Cell_Effected();
            _effectEventNetworkService.RaiseEventAddEffect(_serializableEffects.Freeze3Cell_Effected);
        }

        public void FreezeFigure(Vector2Int chosenCell, CardInfo info)
        {
            List<Cell> allCells =
                _fieldFigureService.GetAllCellWithFigure((CellFigure) _playerService.GetCurrentPlayer().SideId);
            List<Cell> resultCells = new List<Cell>();
            while (allCells.Count > 0 && resultCells.Count < 5)
            {
                int randValue = Random.Range(0, allCells.Count);
                resultCells.Add(allCells[randValue]);
                allCells.Remove(allCells[randValue]);
            }

            for (int i = 0; i < resultCells.Count; i++)
            {
                _fieldFigureService.FreezeCell(resultCells[i].Id);


                _serializableEffects.FreezeCell_Effect(resultCells[i].Id);
                _effectEventNetworkService.RaiseEventAddFreezeEffect(resultCells[i].Id);
                /**/
            }
        }

        public void BreakFreeze(Vector2Int chosenCell, CardInfo info)
        {
            List<Effect> effects = _effectService.GetEffectList().FindAll(x => x.EffectPriority == 2);
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
                int effectIndex = _effectService.GetEffectList().IndexOf(resultEffects[i]);

                _effectService.ClearEffect(_effectService.GetEffectList().IndexOf(resultEffects[i]));
                _effectEventNetworkService.RaiseEventClearEffect(effectIndex);
            }

            _coroutineAwaitService.AddAwaitTime(resultEffects[0].EffectTimeDisable);
            _checkEventNetworkService.RaiseEventAwaitTime(resultEffects[0].EffectTimeDisable);
            _finishLineService.MasterChecker(_playerService.GetCurrentPlayer().SideId);
        }

        public void PlaceAroundFreeze(Vector2Int chosenCell, CardInfo info)
        {
            List<Cell> allFreeze = _fieldFigureService.GetAllCellWithSubState(CellSubState.Freeze);

            foreach (var t in allFreeze)
            {
                List<Cell> neighCell = _fieldService.GetAllEmptyNeighbours(t);
                Debug.Log($"{t.Id} : {neighCell.Count}");
                if (neighCell.Count > 0)
                {
                    int randValue = Random.Range(0, neighCell.Count);
                    _fieldFigureService.FreezeCell(neighCell[randValue].Id);

                    _serializableEffects.FreezeCell_Effect(neighCell[randValue].Id);
                    _effectEventNetworkService.RaiseEventAddFreezeEffect(neighCell[randValue].Id);
                }
            }

            _finishLineService.MasterChecker(_playerService.GetCurrentPlayer().SideId);
        }

        public void FreezeAllMana(Vector2Int chosenCell, CardInfo info)
        {
            while (_manaService.IsEnoughMana(1))
            {
                _manaService.IncreaseMana(-1);
                _manaEventNetworkService.RaiseEventIncreaseMana(-1);
                for (int i = 0; i < 2; i++)
                {
                    Vector2Int result = _aiService.GenerateRandomPosition(_fieldService.GetFieldSize());
                    if (result == new Vector2Int(-1, -1)) break;
                    _fieldFigureService.FreezeCell(result);

                    _serializableEffects.FreezeCell_Effect(result);
                    _effectEventNetworkService.RaiseEventAddFreezeEffect(result);
                }
            }

            _manaUIService.UpdateManaUI();
            _coroutineAwaitService.AddAwaitTime(Cell.AnimationTime);
            _finishLineService.MasterChecker(_playerService.GetCurrentPlayer().SideId);
        }

        public void ManaPerIce(Vector2Int chosenCell, CardInfo info)
        {
            List<Effect> effects = _effectService.GetEffectList().FindAll(x => x.EffectPriority == 2);
            int resultMana = effects.Count / 3;
            _manaService.IncreaseMana(resultMana, true);
            _manaEventNetworkService.RaiseEventIncreaseMana(resultMana, true);
            _manaUIService.UpdateManaUI();
        }

        #endregion


        public bool InvokeActionWithCheck(CardModel cardModel, Vector2Int chosenCell)
        {
            bool timeFlag = cardModel.Stopwatch.ElapsedMilliseconds > 80;
            bool typeFlag;
            bool manaFlag = _manaService.IsEnoughMana(cardModel.Info.CardManacost + cardModel.Info.CardBonusManacost);
            bool animFlag = _coroutineService.GetIsQueueEmpty();
            bool playerFlag = _handPoolView.IsCurrentPlayerOnSlot();

            typeFlag = cardModel.Info.CardType switch
            {
                CardTypeImpact.OnField => _fieldService.IsInFieldHeight(cardModel.GetClearPosition().y),
                CardTypeImpact.OnArea => chosenCell != new Vector2(-1, -1),
                CardTypeImpact.OnAreaWithCheck => chosenCell != new Vector2(-1, -1) &&
                                                  _fieldZoneService.IsZoneEnableToPlace(chosenCell,
                                                      cardModel.Info.CardAreaSize),
                _ => false
            };

            bool isInvokeExist = typeFlag && timeFlag && manaFlag && animFlag && playerFlag;

            if (isInvokeExist)
            {
                _handPoolManipulator.RemoveCard(_playerService.GetCurrentPlayer(), cardModel);
                // ReSharper disable once IntVariableOverflowInUncheckedContext
                int manaValue = -cardModel.Info.CardManacost - cardModel.Info.CardBonusManacost;
                _manaService.IncreaseMana(manaValue);
                _manaEventNetworkService.RaiseEventIncreaseMana(manaValue);
                _manaUIService.UpdateManaUI();

                _serializableActions[cardModel.Info.СardActionId]?.Invoke(chosenCell, cardModel.Info);
                _cardEventNetworkService.RaiseEventCardInvoke(cardModel.Info);
                _historyService.AddHistoryCard(_playerService.GetCurrentPlayer(), cardModel.Info);

                _handPoolView.UpdateCardUI();
            }

            return isInvokeExist;
        }

        private CardActions()
        {
            _serializableActions = new Dictionary<string, Action<Vector2Int, CardInfo>>
            {
                ["PlaceFigureWithAddCard"] = PlaceFigureWithAddCard,
                ["PlaceFigure"] = PlaceFigure,
                ["PlaceRandom5"] = PlaceRandom5,
                ["AddFigure_Effected"] = AddFigure_Effected,
                ["AddBonusMana_Effected"] = AddBonusMana_Effected,
                ["Decrease2MaxMana"] = Decrease2MaxMana,
                ["Increase2MaxMana"] = Increase2MaxMana,
                ["Random2Mana"] = Random2Mana,
                ["DecreaseIncrease2Mana"] = DecreaseIncrease2Mana,
                ["Restore1Mana"] = Restore1Mana,
                ["RestoreAllMana"] = RestoreAllMana,
                ["FreezeCell"] = FreezeCell,
                ["FreezeCellGroup"] = FreezeCellGroup,
                ["Freeze3Cell_Effected"] = Freeze3Cell_Effected,
                ["FreezeFigure"] = FreezeFigure,
                ["BreakFreeze"] = BreakFreeze,
                ["PlaceAroundFreeze"] = PlaceAroundFreeze,
                ["FreezeAllMana"] = FreezeAllMana,
                ["ManaPerIce"] = ManaPerIce
            };
        }
    }
}