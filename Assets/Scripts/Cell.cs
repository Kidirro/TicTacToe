using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Cell : MonoBehaviour
{

    public float CellSize
    {
        get { return _cellSize; }
    }
    private float _cellSize;

    public Vector2 Position
    {
        get { return _position; }
    }
    private Vector2 _position;

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

    [SerializeField] private List<Sprite> _spriteList;
    private SpriteRenderer _sprRend;

    void Awake()
    {
        _transform = transform;
        _collider = GetComponent<BoxCollider2D>();
        _sprRend = GetComponent<SpriteRenderer>();
    }

    public void SetState(int s)
    {
        _sprRend.sprite = _spriteList[s];
        _state = (CellState) s;
    }

    public void SetColliderSize(float s)
    {        
        _collider.size = new Vector2(s, s);
    }

    public void SetTransformSize(float s,float reals,bool instantly = true)
    {
        _cellSize = reals;
        if (instantly) _transform.DOScale(s, 0f);
        else _transform.DOScale(s,3f).SetSpeedBased();
    }

    public void SetTransformPosition(float x, float y, bool instantly = true)
    {
        _position = new Vector2(x, y);
        if (instantly) _transform.DOMove(new Vector2(x, y), 0f);
        else _transform.DOMove(new Vector2(x, y), 3f).SetSpeedBased();
    }

    public void Clicked()
    {
        TurnController.TurnProcess(_id);
    }

}

public enum CellState
{
    empty,
    cross,
    circle,
    block
}
