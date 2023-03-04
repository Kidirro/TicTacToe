using System.Diagnostics;
using Coroutine;
using History;
using Mana;
using Managers;
using Network;
using Players;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using Debug = UnityEngine.Debug;


namespace Cards.CustomType
{
    [RequireComponent(typeof(CardView))]
    public class CardModel : MonoBehaviour
    {
        public CardInfo Info { get; private set; }

        private CardView _cardView;

        private readonly Stopwatch stopWatch = new();


        private void Awake()
        {
            _cardView = GetComponent<CardView>();
        }

        public void SetCardInfo(CardInfo ci)
        {
            Info = ci;
            _cardView.UpdateUI();
        }

        /// <summary>
        /// Начало перетягивания карты
        /// </summary>
        public void BeginDrag()
        {
            _cardView.SetCanvasOverrideSorting(true);
            _cardView.SetTransformScale(1, false);
            _cardView.SetTransformPositionWithFingerDistance(Input.mousePosition,false);
            chosedCell = new Vector2Int(-1, -1);
            SetTransformRotation(0);
            _isSlotsReUpdatePositions = false;
            stopWatch.Reset();
            stopWatch.Start();
            if (Info.IsNeedShowTip)
            {
                string textTip = Info.TipText;
                textTip = I2.Loc.LocalizationManager.TryGetTranslation(Info.TipText, out textTip)
                    ? I2.Loc.LocalizationManager.GetTranslation(Info.TipText)
                    : Info.TipText;
                Debug.Log(textTip);
                _cardTip.ShowTip(textTip, false);
            }
            //if (SlotManager.Instance.IsCurrentPlayerOnSlot) SlotManager.Instance.ShowRechanger();
        }

        /// <summary>
        /// Перетягивание карты
        /// </summary>
        public void OnDrag()
        {
            if (!CardPoolController.Instance.IsCurrentPlayerOnSlot) return;
       
            SetTransformPositionWithFingerDistance(Input.mousePosition,false);

            if (Field.Instance.IsInFieldHeight(vectorFigure.y) && !_isSlotsReUpdatePositions)
            {
                _isSlotsReUpdatePositions = true;
                CardPoolController.Instance.UpdateCardPosition(false, this);
                transform.SetAsLastSibling();
                Debug.Log("Entered");
            }
            /*
                    if (_magnitudePosition != Vector2.zero && (_magnitudePosition - vector).magnitude > _magnitudeCard)
                    {
                        _cardTip.HideTip(Field.Instance.IsInFieldHeight(vectorFigure.y));
                        _magnitudePosition = Vector2.zero;
                    }*/

            if (Info.CardType == CardTypeImpact.OnField) return;

            if (Field.Instance.IsInFieldHeight(vectorFigure.y))
            {
                _cardTip.HideTip(true);
            }
            else
            {
                if (Info.IsNeedShowTip)
                {
                    string textTip = Info.TipText;
                    textTip = I2.Loc.LocalizationManager.TryGetTranslation(Info.TipText, out textTip)
                        ? I2.Loc.LocalizationManager.GetTranslation(Info.TipText)
                        : Info.TipText;


                    _cardTip.ShowTip(textTip);
                }
            }

            if (_lastAlphaState != !Field.Instance.IsInFieldHeight(vectorFigure.y))
            {
                _lastAlphaState = !_lastAlphaState;
                SetGroupAlpha(_cardCanvasGroup.alpha, _lastAlphaState ? 1 : 0, false);
            }

            chosedCell = Field.Instance.GetIdFromPosition(vectorFigure, false);
            if (_prevChosedCell != chosedCell)
            {
                if (_prevChosedCell != new Vector2Int(-1, -1))
                {
                    Field.Instance.UnhighlightZone(_prevChosedCell, Info.CardAreaSize);
                }

                if (chosedCell != new Vector2Int(-1, -1))
                {
                    Field.Instance.HighlightZone(chosedCell,
                        Info.CardAreaSize,
                        (PlayerManager.Instance.GetCurrentPlayer().SideId == 1)
                            ? Info.CardHighlightP1
                            : Info.CardHighlightP2,
                        Info.CardHighlightColor
                    );
                }

                _prevChosedCell = chosedCell;
            }
        }

        /// <summary>
        /// Конец движения карты от пальца
        /// </summary>
        public void EndDraged()
        {
            if (chosedCell != new Vector2Int(-1, -1))
            {
                Field.Instance.UnhighlightZone(chosedCell, Info.CardAreaSize);
            }

            stopWatch.Stop();
            _cardCanvas.overrideSorting = false;

            bool timeFlag = stopWatch.ElapsedMilliseconds > 80;
            bool typeFlag = false;
            bool manaFlag = _enoughMana.IsEnoughMana(Info.CardManacost + Info.CardBonusManacost);
            bool animFlag = CoroutineQueueController.isQueueEmpty;
            bool playerFlag = CardPoolController.Instance.IsCurrentPlayerOnSlot;
            bool positionFlag =
                Field.Instance.IsInFieldHeight(_cardTransformRect.localPosition.y -
                                               ScreenScaler.ScreenScalerService.Instance.GetHeight(_fingerToCardDistance
                                                   .y));

            switch (Info.CardType)
            {
                case CardTypeImpact.OnField:
                    typeFlag = true;
                    break;
                case CardTypeImpact.OnArea:
                    typeFlag = chosedCell != new Vector2(-1, -1);
                    break;
                case CardTypeImpact.OnAreaWithCheck:
                    typeFlag = chosedCell != new Vector2(-1, -1) &&
                               Field.Instance.IsZoneEnableToPlace(chosedCell, Info.CardAreaSize);
                    break;
            }


            if (typeFlag && timeFlag && manaFlag && animFlag && playerFlag && positionFlag)
            {
                CardPoolController.Instance.RemoveCard(PlayerManager.Instance.GetCurrentPlayer(), this);
                _increaseMana.IncreaseMana(-Info.CardManacost - Info.CardBonusManacost);
                NetworkEventManager.RaiseEventIncreaseMana(-Info.CardManacost - Info.CardBonusManacost);
                ManaManager.Instance.UpdateManaUI();

                Info.СardAction.Invoke();
                NetworkEventManager.RaiseEventCardInvoke(Info);
                HistoryFactory.Instance.AddHistoryCard(PlayerManager.Instance.GetCurrentPlayer(), Info);

                Info.CardBonusManacost += 1;
                CardPoolController.Instance.UpdateCardUI();
            }
            else
            {
                _lastAlphaState = true;
                SetGroupAlpha(_cardCanvasGroup.alpha, 1, false);
            }

            if (Info.IsNeedShowTip) _cardTip.HideTip(typeFlag && timeFlag && manaFlag);
            CardPoolController.Instance.UpdateCardPosition(false);
        }

        public void CancelDragging()
        {
            if (FindObjectOfType<Field>() != null)
            {
                if (chosedCell != new Vector2Int(-1, -1))
                {
                    Field.Instance.UnhighlightZone(chosedCell, Info.CardAreaSize);
                }
            }

            if (Info.IsNeedShowTip) _cardTip.HideTip(true);
            stopWatch.Stop();
            _cardCanvas.overrideSorting = false;


            _lastAlphaState = true;
            CardPoolController.Instance.UpdateCardUI();
            //SlotManager.Instance.UpdateCardPosition(!gameObject.activeInHierarchy);
        }

        public void SetSideCard(CardInfo info, int side)
        {
            _cardView.SetSideCard(info, side);
        }

        public void UpdateUI(CardInfo info, PlayerInfo playerInfo, bool isNeedLight)
        {
            _cardView.UpdateUI(info, playerInfo, isNeedLight);
        }

        public void SetTransformScale(float reals, bool instantly = true)
        {
            _cardView.SetTransformScale(reals, instantly);
        }

        public void SetTransformParent(Transform parent)
        {
            _cardView.SetTransformParent(parent);
        }

        public void SetSibling(int id)
        {
            _cardView.SetSibling(id);
        }

        public void SetGroupAlpha(float init, float final, bool instantly = true)
        {
            _cardView.SetGroupAlpha(init, final, instantly);
        }

        public void SetTransformPosition(Vector2 position, bool instantly = true)
        {
            _cardView.SetTransformPosition(position, instantly);
        }

        public void SetTransformRotation(float x, bool instantly = true)
        {
            _cardView.SetTransformRotation(x, instantly);
        }

        public void SetCanvasOverrideSorting(bool state)
        {
            _cardView.SetCanvasOverrideSorting(state);
        }

        public void SetTransformPositionWithFingerDistance(Vector2 position, bool instantly = true)
        {
            _cardView.SetTransformPositionWithFingerDistance(position, instantly);
        }
    }
}