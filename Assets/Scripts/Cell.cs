using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{

    [Tooltip("–азмеры клетки в мировых координатах")]
    private float _cellSize;

    public Vector2 Id
    {
        get { return _id; }
        set { _id = value; }
    }
    private Vector2 _id;

    private BoxCollider2D _collider;

    [SerializeField]private List<Sprite> _spriteList;
    private SpriteRenderer _sprRend;

    void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _sprRend = GetComponent<SpriteRenderer>();
        _sprRend.sprite = _spriteList[0];
    }

    public void ChangeSize(float s)
    {
        
        _collider.size = new Vector2(s, s);
    }
}
