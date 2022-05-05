using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeManager : Singleton<ThemeManager>
{
    private Dictionary<CellState, Sprite> _cellSprites = new Dictionary<CellState, Sprite>();

    [SerializeField]
    private Sprite _blankSprite;

    [SerializeField]
    private Sprite _p1Sprite;

    [SerializeField]
    private Sprite _p2Sprite;

   public void Initialization()
    {
        _cellSprites.Add(CellState.empty, _blankSprite);
        _cellSprites.Add(CellState.p1, _p1Sprite);
        _cellSprites.Add(CellState.p2, _p2Sprite);
    }

    public Sprite GetSprite(CellState i)
    {
        return _cellSprites[i];
    }
}
