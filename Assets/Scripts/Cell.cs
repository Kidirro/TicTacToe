using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{

    public float CellSize
    {
        get { return _collider.size.x * transform.localScale.x; }
    }

    public Vector3 Position;

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

    private CellState _state;
    private BoxCollider2D _collider;

    [SerializeField] private List<Sprite> _spriteList;
    private SpriteRenderer _sprRend;

    void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _sprRend = GetComponent<SpriteRenderer>();
    }

    public void ChangeState(int s)
    {
        _sprRend.sprite = _spriteList[s];
        _state = (CellState) s;
    }

    public void ChangeSize(float s)
    {        
        _collider.size = new Vector2(s, s);
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
