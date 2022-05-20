 using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System.Collections.Generic;

public class Card : MonoBehaviour
{

    #region Fields

    /// <summary>
    /// ������� ���������� ��������
    /// </summary>
    [SerializeField]
    private List<GameObject> _manapoints = new List<GameObject>();

    /// <summary>
    /// ���������� ����� ��� �������������� ��������
    /// </summary>
    public CardInfo Info;

    /// <summary>
    /// ����� �������� �����
    /// </summary>
    [SerializeField] private Text _cardDescription;

    /// <summary>
    /// ����������� �����
    /// </summary>
    [SerializeField] private Image _cardImage;

    /// <summary>
    /// ����� ������� � ���� 
    /// ������� ���������
    /// </summary>
    public Vector2 HandPosition;

    /// <summary>
    /// ���������� ������� �����
    /// </summary>
    public Vector2 Position
    {
        get { return _cardPosition; }
    }
    private Vector2 _cardPosition;

    /// <summary>
    /// ���� ��������� 
    /// </summary>
    private RectTransform _transformRect;

    /// <summary>
    /// ���� ������ �������� �����������
    /// </summary>
    private bool _isPositionCoroutineWork = false;


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
    /// ����������
    /// </summary>
    private Stopwatch stopWatch = new Stopwatch();

    #endregion


    void Awake()
    {
        _transformRect = GetComponent<RectTransform>();
    }



    /// <summary>
    /// ������ ������������� �����
    /// </summary>
    public void BeginDrag()
    {
        stopWatch.Reset();
        stopWatch.Start();
    }

    private void OnDisable()
    {
        _isPositionCoroutineWork = false;
        _isSizeCoroutineWork = false;

    }

    private void OnEnable()
    {
        foreach(GameObject go in _manapoints) {
            go.SetActive(false);
        }
        if (Info!=null && Info.CardManacost != 0)
        {
            _manapoints[Info.CardManacost-1].SetActive(true);
        }
    }

    public void SetCardInfo(CardInfo ci)
    {
        Info = ci;
        foreach (GameObject go in _manapoints)
        {
            go.SetActive(false);
        }
        if (Info.CardManacost != 0)
        {
            _manapoints[Info.CardManacost - 1].SetActive(true);
        }
    }

    /// <summary>
    /// ����� �������� ����� �� ������
    /// </summary>
    public void EndDraged()
    {
        Debug.Log(Field.Instance.CheckIsInField(Position));
        Debug.Log(Position);
        stopWatch.Stop();
        if (stopWatch.ElapsedMilliseconds > 80 && Field.Instance.CheckIsInField(Position))
        {
            Info.�ardAction.Invoke();
            SlotManager.Instance.RemoveCard(PlayerManager.Instance.GetCurrentPlayer(), this);
            //Destroy(gameObject);

        }
        else
        {
            Debug.Log("InfoShowed!");
            SetTransformPosition(HandPosition.x, HandPosition.y, false);
        }
    }


    /// <summary>
    /// ������������� �����
    /// </summary>
    public void OnDrag()
    {
        Vector2 vector =Input.mousePosition;
        SetTransformPosition(vector.x, vector.y);
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

    private IEnumerator ScaleIEnumerator()
    {
        _isSizeCoroutineWork = true;
        float prevS = _cardSize;
        float step = (_cardSize - transform.localScale.x) / 100f;
        int i = 0;
        while (i <= 100)
        {
            if (prevS != _cardSize)
            {
                prevS = _cardSize;
                step = (_cardSize - transform.localScale.x) / 100f;
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
        Vector2 step = (prevPos - currentPosition) / 100f;
        int i = 0;
        while (i <= 100)
        {
            currentPosition = _transformRect.localPosition;
            if (prevPos != _cardPosition)
            {
                prevPos = _cardPosition;

                step = (prevPos - currentPosition) / 100f;
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

}
