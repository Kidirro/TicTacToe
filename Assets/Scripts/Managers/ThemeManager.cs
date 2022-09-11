using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{

    public class ThemeManager : Singleton<ThemeManager>
    {
        private Dictionary<CellFigure, Sprite> _cellSprites = new Dictionary<CellFigure, Sprite>();

        [SerializeField]
        private Sprite _blankSprite;

        [SerializeField]
        private Sprite _p1Sprite;

        [SerializeField]
        private Sprite _p2Sprite;

        public void Initialization()
        {
            _cellSprites.Add(CellFigure.none, _blankSprite);
            _cellSprites.Add(CellFigure.p1, _p1Sprite);
            _cellSprites.Add(CellFigure.p2, _p2Sprite);
        }

        public Sprite GetSprite(CellFigure i)
        {
            return _cellSprites[i];
        }
    }
}