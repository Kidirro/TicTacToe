using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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



    public void Clicked()
    {
        Debug.Log("|");
        SetTransformSize(_cardSize + 1,false);
    }

    public void Draged()
    {
        SetTransformSize(_cardSize - 1, false);
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
        Field.Instance.CellAnimating = Field.Instance.CellAnimating + 1;
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
        Field.Instance.CellAnimating = Field.Instance.CellAnimating - 1;
        _isSizeCoroutineWork = false;
        yield break;
    }

    private IEnumerator PositionIEnumerator()
    {
        _isPositionCoroutineWork = true;
        Field.Instance.CellAnimating = Field.Instance.CellAnimating + 1;
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
        Field.Instance.CellAnimating = Field.Instance.CellAnimating - 1;
        _isPositionCoroutineWork = false;
        yield break;
    }

}
