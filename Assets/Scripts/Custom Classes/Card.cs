using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using TMPro;
using Debug = UnityEngine.Debug;
using System.Collections.Generic;

public class Card : MonoBehaviour
{

    #region Fields

    /// <summary>
    /// 
    /// </summary>
    [HideInInspector]
    public static Vector2Int ChosedCell;

    /// <summary>
    /// �������� ��������
    /// </summary>
    const float _rotationSpeed = 8;

    /// <summary>
    /// �������� ��������
    /// </summary>
    const float _scaleSpeed = 8;


    /// <summary>
    /// �������� �����������
    /// </summary>
    const float _positionSpeed = 8;


    /// <summary>
    /// ����� ���������
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI _manapoints;

    /// <summary>
    /// ���������� ����� ��� �������������� ��������
    /// </summary>
    [HideInInspector]
    public CardInfo Info;

    /// <summary>
    /// ������ �����
    /// </summary>
    [SerializeField]
    private GameObject _cardObj;

    /// <summary>
    /// ����� �������� �����
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI _cardDescription;

    /// <summary>
    /// ����������� �����
    /// </summary>
    [SerializeField]
    private Image _cardImage;


    /// <summary>
    /// ����������� �����
    /// </summary>
    [SerializeField]
    private List<GameObject> _bonusImageList = new List<GameObject>();

    /// <summary>
    /// ����� ������� � ���� 
    /// ������� ���������
    /// </summary>
    [HideInInspector]
    public Vector2 HandPosition;

    /// <summary>
    /// ����� ������� � ���� 
    /// ������� ���������
    /// </summary>
    [HideInInspector]
    public float HandRotation;

    /// <summary>
    /// ���������� ������� �����
    /// </summary>
    public Vector2 Position
    {
        get { return _cardPosition; }
    }
    private Vector2 _cardPosition;

    /// <summary>
    /// ���������� ������� �����
    /// </summary>
    public float Rotation
    {
        get { return _cardRotation; }
    }
    private float _cardRotation;

    /// <summary>
    /// ���� ��������� 
    /// </summary>
    private RectTransform _transformRect;

    /// <summary>
    /// ���������� ������ �����
    /// </summary>
    public float Size
    {
        get { return _cardSize; }
    }
    private float _cardSize;

    /// <summary>
    /// ���� ������ �������� �������
    /// </summary>
    private bool _isSizeCoroutineWork = false;

    /// <summary>
    /// ���� ������ �������� �����������
    /// </summary>
    private bool _isPositionCoroutineWork = false;

    /// <summary>
    /// ���� ������ �������� �����������
    /// </summary>
    private bool _isRotationCoroutineWork = false;

    /// <summary>
    /// ����������
    /// </summary>
    private Stopwatch stopWatch = new Stopwatch();
    

    /// <summary>
    /// ���������� ������� �����
    /// </summary>
    private Vector2Int _prevPosition;

    /// <summary>
    /// ������� ������� ��� 
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



    #endregion


    void Awake()
    {
        _transformRect = GetComponent<RectTransform>();
        _canvas = GetComponent<Canvas>();
    }

    private void OnDisable()
    {
        _isPositionCoroutineWork = false;
        _isSizeCoroutineWork = false;
        _isRotationCoroutineWork = false;

    }

    private void OnEnable()
    {
        _canvas.overrideSorting = false;
        if (Info)
        {
            _manapoints.text = Info.CardManacost.ToString();
            SetSideCard(1);
            _cardDescription.text = Info.CardDescription;

            _cardObj.SetActive(true);
            _cardObj.transform.localScale = new Vector3(ScreenManager.Instance.GetWidthRatio(), ScreenManager.Instance.GetWidthRatio());

            for (int i = 0; i < _bonusImageList.Count; i++)
            {
                _bonusImageList[i].SetActive(i + 1 == (int)Info.CardBonus);
            }
        }
    }

    public void SetCardInfo(CardInfo ci)
    {
        Info = ci;
        _manapoints.text = Info.CardManacost.ToString();
        SetSideCard(1);
        _cardDescription.text = Info.CardDescription;
        for (int i = 0; i < _bonusImageList.Count; i++)
        {
            _bonusImageList[i].SetActive(i + 1 == (int)Info.CardBonus);
        }
    }

    public void SetSideCard(int side)
    {
        _cardImage.sprite = (side == 1) ? Info.CardImageP1 : Info.CardImageP2;
    }


    /// <summary>
    /// ������ ������������� �����
    /// </summary>
    public void BeginDrag()
    {
        _canvas.overrideSorting = true;
        SetTransformSize(1, false);
        Vector2 positionVect = Input.mousePosition + new Vector3(ScreenManager.Instance.GetWidth(_fingerDistance.x),ScreenManager.Instance.GetHeight(_fingerDistance.y));
        SetTransformPosition(positionVect.x,positionVect.y,false);
        ChosedCell = new Vector2Int(-1, -1);
        SetTransformRotation(0);
        _isSlotReInit = false;
        stopWatch.Reset();
        stopWatch.Start();
        SlotManager.Instance.ShowRechanger();
    }

    /// <summary>
    /// ������������� �����
    /// </summary>
    public void OnDrag()
    {
        Vector2 vector = Input.mousePosition + new Vector3(ScreenManager.Instance.GetWidth(_fingerDistance.x), ScreenManager.Instance.GetHeight(_fingerDistance.y));
        Vector2 vectorFigure = Input.mousePosition + new Vector3(ScreenManager.Instance.GetWidth(_fingerDistance.x), ScreenManager.Instance.GetHeight(_fingerDistance.y/2));

        SetTransformPosition(vector.x, vector.y);
        
        if (Field.Instance.IsInFieldHeight(vectorFigure.y) && !_isSlotReInit)
        {
            _isSlotReInit = true;
            SlotManager.Instance.UpdateCardPosition(false, this);
            transform.SetAsLastSibling();
            Debug.Log("Entered");
        }


        if (Info.CardType == CardTypeImpact.OnField) return;

        _cardObj.SetActive(!Field.Instance.IsInFieldHeight(vectorFigure.y));

        ChosedCell = Field.Instance.GetIdFromPosition(vectorFigure, false);
        if (_prevPosition != ChosedCell)
        {
            if (_prevPosition != new Vector2Int(-1, -1))
            {
                Field.Instance.UnHighlightZone(_prevPosition, Info.CardAreaSize);
            }

            if (ChosedCell != new Vector2Int(-1, -1))
            {
                Field.Instance.HighlightZone(ChosedCell, Info.CardAreaSize);
            }
            _prevPosition = ChosedCell;
        }
    }

    /// <summary>
    /// ����� �������� ����� �� ������
    /// </summary>
    public void EndDraged()
    {
        if (ChosedCell != new Vector2Int(-1, -1))
        {
            Field.Instance.UnHighlightZone(ChosedCell, Info.CardAreaSize);
        }
        SlotManager.Instance.HideRechanger();
        SetTransformSize(0.7f, false);
        stopWatch.Stop();
        _canvas.overrideSorting = false;

        if (SlotManager.Instance.IsOnRechanger(_cardPosition.y - ScreenManager.Instance.GetHeight(_fingerDistance.y)))
        {
            SlotManager.Instance.UseRechanger(this);
            return;
        }

        bool TimeFlag = stopWatch.ElapsedMilliseconds > 80;
        bool TypeFlag = false;
        bool ManaFlag = ManaManager.Instance.IsEnoughMana(Info.CardManacost);

        switch (Info.CardType){
            case CardTypeImpact.OnField:
                TypeFlag = Field.Instance.IsInFieldHeight(_cardPosition.y);
                break;
            case CardTypeImpact.OnArea:
                TypeFlag = Field.Instance.IsInFieldHeight(_cardPosition.y) && ChosedCell!= new Vector2(-1,-1);
                break;
            case CardTypeImpact.OnAreaWithCheck:
                TypeFlag = Field.Instance.IsInFieldHeight(_cardPosition.y) && ChosedCell != new Vector2(-1, -1) && Field.Instance.IsZoneEmpty(ChosedCell,Info.CardAreaSize);
                break;
        }

        if (TypeFlag && TimeFlag&& ManaFlag)
        {
            SlotManager.Instance.RemoveCard(PlayerManager.Instance.GetCurrentPlayer(), this);
            SlotManager.Instance.UpdateCardPosition(false);
            ManaManager.Instance.DecreaseMana(Info.CardManacost);
            ManaManager.Instance.UpdateManaUI();

            Info.�ardAction.Invoke();
        }
        else
        {
            _cardObj.SetActive(true);
        }
        SlotManager.Instance.UpdateCardPosition(false);
    }

    /// <summary>
    /// ��������� ����������� ������� �����
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
    /// ���������� ������������ ������
    /// </summary>
    /// <param name="parent"></param>
    public void SetTransformParent(Transform parent)
    {
        _transformRect.SetParent(parent);
        SetTransformPosition(0, 0);
        _transformRect.localScale = Vector3.one;
    }


    /// <summary>
    /// ��������� ����������� ��������� �����
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
    /// ��������� ����������� �������� �����
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

    private IEnumerator ScaleIEnumerator()
    {
        _isSizeCoroutineWork = true;
        float prevS = _cardSize;
        float countStep = 100f / _scaleSpeed;
        float step = (_cardSize - transform.localScale.x) / countStep;
        int i = 0;
        while (i <= countStep)
        {
            if (prevS != _cardSize)
            {
                prevS = _cardSize;
                step = (_cardSize - transform.localScale.x) / countStep;
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

        float countStep = 100f / _positionSpeed;

        Vector2 prevPos = _cardPosition;

        Vector2 currentPosition = _transformRect.localPosition;
        Vector2 step = (prevPos - currentPosition) / countStep;
        int i = 0;
        while (i <= countStep)
        {
            currentPosition = _transformRect.localPosition;
            if (prevPos != _cardPosition)
            {
                prevPos = _cardPosition;

                step = (prevPos - currentPosition) / countStep;
                i = 0;
            }

            _transformRect.localPosition = currentPosition + step;
            i++;
            yield return null;
        }
        _transformRect.localPosition = _cardPosition;
        _isPositionCoroutineWork = false;
        yield break;
    }

    private IEnumerator RotationIEnumerator()
    {
        _isRotationCoroutineWork = true;


        float countStep = 100f / _rotationSpeed;

        Quaternion prevRot = Quaternion.Euler(0, 0, _cardRotation);

        Quaternion currentRotation = _transformRect.localRotation;
        int i = 0;
        while (i <= countStep)
        {
            if (prevRot != Quaternion.Euler(0, 0, _cardRotation))
            {
                prevRot = Quaternion.Euler(0, 0, _cardRotation);
                currentRotation = _transformRect.localRotation;
                i = 0;
            }

            _transformRect.localRotation = Quaternion.Lerp(currentRotation, prevRot, (float)i / countStep);
            i++;
            yield return null;
        }
        _transformRect.localRotation = Quaternion.Euler(0, 0, _cardRotation);
        _isRotationCoroutineWork = false;
        yield break;
    }
}
