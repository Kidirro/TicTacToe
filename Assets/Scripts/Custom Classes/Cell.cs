using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Cell : MonoBehaviour
{
    const float _scaleSpeed = 4;
    const float _positionSpeed = 4;

    public float CellSize
    {
        get { return _cellSize; }
    }
    private float _cellSize;

    private float _realSize;
    private bool _isSizeCoroutineWork = false;

    public Vector2 Position
    {
        get { return _position; }
    }
    private Vector2 _position;
    private bool _isPositionCoroutineWork = false;

    public Vector2Int Id
    {
        get { return _id; }
        set { _id = value; }
    }
    private Vector2Int _id;

    public CellState State
    {
        get { return _state; }
    }

    private Transform _transform;

    private CellState _state;

    private BoxCollider2D _collider;


    private SpriteRenderer _sprRend;

    void Awake()
    {
        _transform = transform;
        _collider = GetComponent<BoxCollider2D>();
        _sprRend = GetComponent<SpriteRenderer>();
    }

    public void SetState(int s)
    {
        _sprRend.sprite = ThemeManager.Instance.GetSprite((CellState) s);
        _state = (CellState) s;
    }

    public void SetState(CellState s)
    {
        _sprRend.sprite = ThemeManager.Instance.GetSprite(s);
        _state = s;
    }

    public void SetColliderSize(float s)
    {        
        _collider.size = new Vector2(s, s);
    }

    public void SetTransformSize(float s,float reals,bool instantly = true)
    {
        _cellSize = reals;
        _realSize = s;
        if (instantly) _transform.localScale=new Vector2(_realSize, _realSize);
        else if (!_isSizeCoroutineWork) StartCoroutine(ScaleIEnumerator());
    }

    public void SetTransformPosition(float x, float y, bool instantly = true)
    {
        _position = new Vector2(x, y);
        if (instantly) _transform.position = _position;
        else if (!_isPositionCoroutineWork) StartCoroutine(PositionIEnumerator());
    }

    public void Clicked()
    {
            TurnController.TurnProcess(_id);
    }

    private IEnumerator ScaleIEnumerator()
    {
        _isSizeCoroutineWork = true;
        Field.Instance.CellAnimating = Field.Instance.CellAnimating + 1;
        float prevS = _realSize;
        float step = (_realSize - _transform.localScale.x) / 100f*_scaleSpeed;
        int i = 0;
        while (i <= 100/ _scaleSpeed)
        {
            if (prevS != _realSize)
            {
                 prevS = _realSize;
                 step = (_realSize - _transform.localScale.x) / 100f* _scaleSpeed;
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
        Vector2 prevPos = _position ;

        Vector2 currentPosition = _transform.position;
        Vector2 step = (prevPos - currentPosition) / 100f * _positionSpeed;
        int i = 0;
        while (i <= 100/ _positionSpeed)
        {
            currentPosition = _transform.position;
            if (prevPos != _position)
            {
                prevPos = _position;

                step = (prevPos - currentPosition) / 100f * _positionSpeed;
                i = 0;
            }

            _transform.position= currentPosition+step;
            i++;
            yield return null;
        }
        _transform.position =_position;
        Field.Instance.CellAnimating = Field.Instance.CellAnimating- 1;
        _isPositionCoroutineWork = false;
        yield break;
    }


}

public enum CellState
{
    block=-1,
    empty =0,
    p1 =1,
    p2= 2
}
