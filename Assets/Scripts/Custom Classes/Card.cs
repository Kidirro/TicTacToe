 using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class Card : MonoBehaviour
{
    public CardInfo Info;
    [SerializeField] private Text _cardDescription;
    [SerializeField] private Image _cardImage;


    public Vector2 HandPosition;

    public Vector2 Position
    {
        get { return _cardPosition; }
    }
    private Vector2 _cardPosition;
    private bool _isPositionCoroutineWork = false;

    void Awake()
    {
        _transform = this.transform;
    }

    public float Size
    {
        get { return _cardSize; }
    }
    private float _cardSize;
    private bool _isSizeCoroutineWork = false;

    private Transform _transform;

    private Stopwatch stopWatch = new Stopwatch();

    private Field _field = Field.Instance;
 
    public void BeginDrag()
    {
        stopWatch.Reset();
        stopWatch.Start();
    }

    public void EndDraged()
    {
        stopWatch.Stop();
        Debug.Log(stopWatch.ElapsedMilliseconds);
        if (stopWatch.ElapsedMilliseconds > 80 && _field.CheckIsInField(Position))
        {
            Info.ÑardAction.Invoke();
            Destroy(gameObject);

        }
        else
        {
            Debug.Log("InfoShowed!");
        }
        SetTransformPosition(HandPosition.x, HandPosition.y, false);
    }

    public void OnDrag()
    {
        Vector2 vector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        SetTransformPosition(vector.x, vector.y);
    }


    public void SetTransformSize(float reals, bool instantly = true)
    {
        _cardSize = reals;
        if (instantly) _transform.localScale = new Vector2(reals, reals);
        else if (!_isSizeCoroutineWork) StartCoroutine(ScaleIEnumerator());
    }

    public void SetTransformPosition(float x, float y, bool instantly = true)
    {
        _cardPosition = new Vector2(x, y);
        if (instantly) _transform.position = _cardPosition;
        else if (!_isPositionCoroutineWork) StartCoroutine(PositionIEnumerator());
    }

    private IEnumerator ScaleIEnumerator()
    {
        _isSizeCoroutineWork = true;
        float prevS = _cardSize;
        float step = (_cardSize - _transform.localScale.x) / 100f;
        int i = 0;
        while (i <= 100)
        {
            if (prevS != _cardSize)
            {
                prevS = _cardSize;
                step = (_cardSize - _transform.localScale.x) / 100f;
                i = 0;
            }
            _transform.localScale = _transform.localScale + new Vector3(step, step);
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

        Vector2 currentPosition = _transform.position;
        Vector2 step = (prevPos - currentPosition) / 100f;
        int i = 0;
        while (i <= 100)
        {
            currentPosition = _transform.position;
            if (prevPos != _cardPosition)
            {
                prevPos = _cardPosition;

                step = (prevPos - currentPosition) / 100f;
                i = 0;
            }

            _transform.position = currentPosition + step;
            i++;
            yield return null;
        }
        _transform.position = _cardPosition;
        _isPositionCoroutineWork = false;
        yield break;
    }

}
