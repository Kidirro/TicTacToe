using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using TMPro;
using Debug = UnityEngine.Debug;
using System.Collections.Generic;
using Managers;

public class Card : MonoBehaviour
{

    #region Fields

    /// <summary>
    /// 
    /// </summary>
    [HideInInspector]
    public static Vector2Int ChosedCell = new Vector2Int(-1, -1);

    /// <summary>
    /// Скорость поворота
    /// </summary>
    const int _rotationCountFrame = 12;

    /// <summary>
    /// Скорость поворота
    /// </summary>
    const int _scaleCountFrame = 12;

    /// <summary>
    /// Скорость перемещения
    /// </summary>
    const int _positionCountFrame = 12;

    /// <summary>
    /// Скорость альфы подсветки
    /// </summary>
    const int _lightCountFrame = 12;
    
    /// <summary>
    /// Скорость альфы карты
    /// </summary>
    const int _canvasAlphaCountFrame = 12;

    /// <summary>
    /// Текст манакоста
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI _manapoints;

    /// <summary>
    /// Информация карты как информационной сущности
    /// </summary>
    [HideInInspector]
    public CardInfo Info;

    /// <summary>
    /// Обьект карты
    /// </summary>
    [SerializeField]
    private CanvasGroup _cardCanvas;

    /// <summary>
    /// Обьект подсветки
    /// </summary>
    [SerializeField]
    private Image _cardLight;

    /// <summary>
    /// Текст описания карты
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI _cardDescription;

    /// <summary>
    /// Изображение карты
    /// </summary>
    [SerializeField]
    private Image _cardImage;

    /// <summary>
    /// Подсказка
    /// </summary>
    [SerializeField]
    private CardTips _cardTip;


    /// <summary>
    /// Изображение карты
    /// </summary>
    [SerializeField]
    private List<GameObject> _bonusImageList = new List<GameObject>();

    /// <summary>
    /// Место позиции в руке 
    /// </summary>
    [HideInInspector]
    public Vector2 HandPosition;

    /// <summary>
    /// Место позиции в руке 
    /// </summary>
    [HideInInspector]
    public float HandRotation;

    /// <summary>
    /// Магнитуда ло удаления подсказки
    /// </summary>
    [SerializeField]
    private float _magnitudeCard;

    /// <summary>
    /// Актуальная позиция карты
    /// </summary>
    public Vector2 Position
    {
        get { return _cardPosition; }
    }
    private Vector2 _cardPosition;

    /// <summary>
    /// Актуальная позиция карты
    /// </summary>
    public float Rotation
    {
        get { return _cardRotation; }
    }
    private float _cardRotation;

    /// <summary>
    /// Рект трансформ 
    /// </summary>
    private RectTransform _transformRect;

    /// <summary>
    /// Актуальный размер карты
    /// </summary>
    public float Size
    {
        get { return _cardSize; }
    }
    private float _cardSize;

    /// <summary>
    /// Флаг работы карутины размера
    /// </summary>
    private bool _isSizeCoroutineWork = false;

    /// <summary>
    /// Флаг работы карутины перемещения
    /// </summary>
    private bool _isPositionCoroutineWork = false;

    /// <summary>
    /// Флаг работы карутины перемещения
    /// </summary>
    private bool _isRotationCoroutineWork = false;

    /// <summary>
    /// Секундомер
    /// </summary>
    private Stopwatch stopWatch = new Stopwatch();


    /// <summary>
    /// Предыдущая позиция карты
    /// </summary>
    private Vector2Int _prevPosition;

    /// <summary>
    /// Канавас объекта для 
    /// </summary>
    private Canvas _canvas;

    /// <summary>
    /// 
    /// </summary>
    private bool _isSlotReInit = false;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private Vector2 _fingerDistance;

    private bool _lastState = true;

    private Coroutine _alphaCoroutine;

    #endregion


    void Awake()
    {
        _transformRect = GetComponent<RectTransform>();
        _canvas = GetComponent<Canvas>();
    }

    private void OnDisable()
    {
        //CancelDragging();
        _isPositionCoroutineWork = false;
        _isSizeCoroutineWork = false;
        _isRotationCoroutineWork = false;

    }

    private void OnEnable()
    {
        _canvas.overrideSorting = false;
        if (Info)
        {
            UpdateUI();
            SetGroupAlpha(_cardCanvas.alpha, 1);
            _cardTip.HideTip(true);
            _cardCanvas.transform.localScale = new Vector3(ScreenManager.Instance.GetWidthRatio(), ScreenManager.Instance.GetWidthRatio());
            _cardTip.transform.localScale = new Vector3(ScreenManager.Instance.GetWidthRatio(), ScreenManager.Instance.GetWidthRatio());
            _lastState = true;
        }
    }

    public void SetCardInfo(CardInfo ci)
    {
        Info = ci;
        UpdateUI();
    }

    public void UpdateUI()
    {
        _manapoints.text = (Info.CardManacost + Info.CardBonusManacost).ToString();
        SetSideCard(1);

        Debug.Log($"UpdatedUICARD: {ManaManager.Instance.IsEnoughMana(Info.CardManacost + Info.CardBonusManacost)}. {Info.CardManacost}. {ManaManager.Instance.CurrentMana}");

        string desc = "";
        desc = I2.Loc.LocalizationManager.TryGetTranslation(Info.CardDescription, out desc) ? I2.Loc.LocalizationManager.GetTranslation(Info.CardDescription) : Info.CardDescription;

        bool manaFlag = ManaManager.Instance.IsEnoughMana(Info.CardManacost + Info.CardBonusManacost);
        bool playerFlag = (SlotManager.Instance.CurrentPlayerSet != null) &&
            (PlayerManager.Instance.GetCurrentPlayer() != null) &&
            (SlotManager.Instance.CurrentPlayerSet.SideId == PlayerManager.Instance.GetCurrentPlayer().SideId);
        StartCoroutine(_cardLight.AlphaWithLerp(_cardLight.color.a, (playerFlag && manaFlag) ? 1 : 0, _lightCountFrame));
        //_cardLight.color = new Color(_cardLight.color.r, _cardLight.color.g, _cardLight.color.b, ManaManager.Instance.IsEnoughMana(Info.CardManacost + Info.CardBonusManacost) ? 1 : 0);
        _cardDescription.text = desc;

        for (int i = 0; i < _bonusImageList.Count; i++)
        {
            _bonusImageList[i].SetActive(i == (int)Info.CardBonus);
        }
    }

    public void SetSideCard(int side)
    {
        _cardImage.sprite = (side == 1) ? Info.CardImageP1 : Info.CardImageP2;
    }


    /// <summary>
    /// Начало перетягивания карты
    /// </summary>
    public void BeginDrag()
    {
        _canvas.overrideSorting = true;
        SetTransformSize(1, false);
        Vector2 positionVect = Input.mousePosition + new Vector3(ScreenManager.Instance.GetWidth(_fingerDistance.x), ScreenManager.Instance.GetHeight(_fingerDistance.y));
        SetTransformPosition(positionVect.x, positionVect.y, false);
        ChosedCell = new Vector2Int(-1, -1);
        SetTransformRotation(0);
        _isSlotReInit = false;
        stopWatch.Reset();
        stopWatch.Start();
        if (Info.IsNeedShowTip)
        {
            string textTip = Info.TipText;
            textTip = I2.Loc.LocalizationManager.TryGetTranslation(Info.TipText, out textTip) ? I2.Loc.LocalizationManager.GetTranslation(Info.TipText) : Info.TipText;
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
        if (!SlotManager.Instance.IsCurrentPlayerOnSlot) return;
        Vector2 vector = Input.mousePosition + new Vector3(ScreenManager.Instance.GetWidth(_fingerDistance.x), ScreenManager.Instance.GetHeight(_fingerDistance.y));
        Vector2 vectorFigure = Input.mousePosition + new Vector3(ScreenManager.Instance.GetWidth(_fingerDistance.x), ScreenManager.Instance.GetHeight(_fingerDistance.y / 2));

        SetTransformPosition(vector.x, vector.y);

        if (Field.Instance.IsInFieldHeight(vectorFigure.y) && !_isSlotReInit)
        {
            _isSlotReInit = true;
            SlotManager.Instance.UpdateCardPosition(false, this);
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
                textTip = I2.Loc.LocalizationManager.TryGetTranslation(Info.TipText, out textTip) ? I2.Loc.LocalizationManager.GetTranslation(Info.TipText) : Info.TipText;


                _cardTip.ShowTip(textTip);
            }
        }

        if (_lastState != !Field.Instance.IsInFieldHeight(vectorFigure.y))
        {
            _lastState = !_lastState;
            SetGroupAlpha(_cardCanvas.alpha, _lastState ? 1 : 0, false);
        }

        ChosedCell = Field.Instance.GetIdFromPosition(vectorFigure, false);
        if (_prevPosition != ChosedCell)
        {
            if (_prevPosition != new Vector2Int(-1, -1))
            {
                Field.Instance.UnhighlightZone(_prevPosition, Info.CardAreaSize);
            }

            if (ChosedCell != new Vector2Int(-1, -1))
            {
                Field.Instance.HighlightZone(ChosedCell,
                                                Info.CardAreaSize,
                                                (PlayerManager.Instance.GetCurrentPlayer().SideId == 1) ? Info.CardHighlightP1 : Info.CardHighlightP2,
                                                Info.CardHighlightColor
                                              );
            }
            _prevPosition = ChosedCell;
        }
    }

    /// <summary>
    /// Конец движения карты от пальца
    /// </summary>
    public void EndDraged()
    {

        if (ChosedCell != new Vector2Int(-1, -1))
        {
            Field.Instance.UnhighlightZone(ChosedCell, Info.CardAreaSize);
        }
        //SlotManager.Instance.HideRechanger();
        stopWatch.Stop();
        _canvas.overrideSorting = false;

        /*    if (SlotManager.Instance.IsOnRechanger(_cardPosition.y - ScreenManager.Instance.GetHeight(_fingerDistance.y)))
            {
                SlotManager.Instance.UseRechanger(this);
                return;
            }*/

        bool TimeFlag = stopWatch.ElapsedMilliseconds > 80;
        bool TypeFlag = false;
        bool ManaFlag = ManaManager.Instance.IsEnoughMana(Info.CardManacost + Info.CardBonusManacost);
        bool AnimFlag = CoroutineManager.IsQueueEmpty;
        bool PlayerFlag = SlotManager.Instance.IsCurrentPlayerOnSlot;

        switch (Info.CardType)
        {
            case CardTypeImpact.OnField:
                TypeFlag = Field.Instance.IsInFieldHeight(_cardPosition.y);
                break;
            case CardTypeImpact.OnArea:
                TypeFlag = Field.Instance.IsInFieldHeight(_cardPosition.y) && ChosedCell != new Vector2(-1, -1);
                break;
            case CardTypeImpact.OnAreaWithCheck:
                TypeFlag = Field.Instance.IsInFieldHeight(_cardPosition.y) && ChosedCell != new Vector2(-1, -1) && Field.Instance.IsZoneEnableToPlace(ChosedCell, Info.CardAreaSize);
                break;
        }

        if (TypeFlag && TimeFlag && ManaFlag && AnimFlag && PlayerFlag)
        {
            SlotManager.Instance.RemoveCard(PlayerManager.Instance.GetCurrentPlayer(), this);
            ManaManager.Instance.IncreaseMana(-Info.CardManacost - Info.CardBonusManacost);
            NetworkEventManager.RaiseEventIncreaseMana(-Info.CardManacost - Info.CardBonusManacost);
            ManaManager.Instance.UpdateManaUI();

            Info.СardAction.Invoke();
            NetworkEventManager.RaiseEventCardInvoke(Info);
            HistoryManager.Instance.AddHistoryCard(PlayerManager.Instance.GetCurrentPlayer(), Info);

            Info.CardBonusManacost += 1;
            SlotManager.Instance.UpdateCardUI();

        }
        else
        {
            _lastState = true;
            SetGroupAlpha(_cardCanvas.alpha, 1, false);
        }
        if (Info.IsNeedShowTip) _cardTip.HideTip(TypeFlag && TimeFlag && ManaFlag);
        SlotManager.Instance.UpdateCardPosition(false);
    }

    /// <summary>
    /// Изменение актуального размера карты
    /// </summary>
    /// <param name="reals"></param>
    /// <param name="instantly"></param>
    public void SetTransformSize(float reals, bool instantly = true)
    {
        _cardSize = reals;
        if (instantly) transform.localScale = new Vector2(reals, reals);
        else if (!_isSizeCoroutineWork) StartCoroutine(ScaleIEnumerator());
    }

    /// <summary>
    /// Установить родительский обьект
    /// </summary>
    /// <param name="parent"></param>
    public void SetTransformParent(Transform parent)
    {
        _transformRect.SetParent(parent);
        SetTransformPosition(0, 0);
        _transformRect.localScale = Vector3.one;
    }

    /// <summary>
    /// Изменение актуального положения карты
    /// </summary>
    /// <param name="reals"></param>
    /// <param name="instantly"></param>
    public void SetGroupAlpha(float init, float final, bool instantly = true)
    {
        if (_alphaCoroutine != null) StopCoroutine(_alphaCoroutine);
        if (instantly) _cardCanvas.alpha = final;
        else _alphaCoroutine = StartCoroutine(_cardCanvas.AlphaWithLerp(init, final, _canvasAlphaCountFrame));
    }


    /// <summary>
    /// Изменение актуального положения карты
    /// </summary>
    /// <param name="reals"></param>
    /// <param name="instantly"></param>
    public void SetTransformPosition(float x, float y, bool instantly = true)
    {
        _cardPosition = new Vector2(x, y);
        if (instantly) _transformRect.localPosition = _cardPosition;
        else if (!_isPositionCoroutineWork) StartCoroutine(PositionIEnumerator());
    }

    /// <summary>
    /// Изменение актуального поворота карты
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="instantly"></param>
    public void SetTransformRotation(float x, bool instantly = true)
    {
        _cardRotation = x;
        if (instantly) _transformRect.localRotation = Quaternion.Euler(0, 0, x);
        else if (!_isRotationCoroutineWork) StartCoroutine(RotationIEnumerator());
    }

    public void CancelDragging()
    {
        if (FindObjectOfType<Field>() != null)
        {
            if (ChosedCell != new Vector2Int(-1, -1))
            {
                Field.Instance.UnhighlightZone(ChosedCell, Info.CardAreaSize);
            }
        }

        if (Info.IsNeedShowTip) _cardTip.HideTip(true);
        stopWatch.Stop();
        _canvas.overrideSorting = false;


        _lastState = true;
        SlotManager.Instance.UpdateCardUI();
        //SlotManager.Instance.UpdateCardPosition(!gameObject.activeInHierarchy);
    }

    private IEnumerator ScaleIEnumerator()
    {
        _isSizeCoroutineWork = true;
        float prevS = _cardSize;
        float step = (_cardSize - transform.localScale.x) / _scaleCountFrame;
        int i = 0;
        while (i < _scaleCountFrame)
        {
            if (prevS != _cardSize)
            {
                prevS = _cardSize;
                step = (_cardSize - transform.localScale.x) / _scaleCountFrame;
                i = 0;
            }
            transform.localScale = transform.localScale + new Vector3(step, step);
            i++;
            yield return null;
        }
        _isSizeCoroutineWork = false;
        yield break;
    }

    private IEnumerator PositionIEnumerator()
    {
        _isPositionCoroutineWork = true;

        Vector2 prevPos = _cardPosition;

        Vector2 currentPosition = _transformRect.localPosition;
        Vector2 step = (prevPos - currentPosition) / _positionCountFrame;
        int i = 0;
        while (i < _positionCountFrame)
        {
            currentPosition = _transformRect.localPosition;
            if (prevPos != _cardPosition)
            {
                prevPos = _cardPosition;

                step = (prevPos - currentPosition) / _positionCountFrame;
                i = 0;
            }

            _transformRect.localPosition = currentPosition + step;
            i++;
            yield return null;
        }
        _transformRect.localPosition = _cardPosition;
        _isPositionCoroutineWork = false;
    }

    private IEnumerator RotationIEnumerator()
    {
        _isRotationCoroutineWork = true;

        Quaternion prevRot = Quaternion.Euler(0, 0, _cardRotation);

        Quaternion currentRotation = _transformRect.localRotation;
        int i = 0;
        while (i < _rotationCountFrame)
        {
            if (prevRot != Quaternion.Euler(0, 0, _cardRotation))
            {
                prevRot = Quaternion.Euler(0, 0, _cardRotation);
                currentRotation = _transformRect.localRotation;
                i = 0;
            }

            _transformRect.localRotation = Quaternion.Lerp(currentRotation, prevRot, (float)i / _rotationCountFrame);
            i++;
            yield return null;
        }
        _transformRect.localRotation = Quaternion.Euler(0, 0, _cardRotation);
        _isRotationCoroutineWork = false;
        yield break;
    }
}
