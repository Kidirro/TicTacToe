using System;
using System.Collections.Generic;
using Theme.Interfaces;
using UnityEngine;

namespace Theme
{

    public class ThemeManager : MonoBehaviour, IThemeService
    {
        private readonly Dictionary<CellFigure, Sprite> _cellSprites = new Dictionary<CellFigure, Sprite>();

        private readonly Dictionary<CellSubState, Sprite> _subStateSprites = new Dictionary<CellSubState, Sprite>();

        [SerializeField]
        private Sprite _blankSprite;

        [SerializeField]
        private Sprite _p1Sprite;

        [SerializeField]
        private Sprite _p2Sprite;

        [SerializeField]
        private Sprite _freezeSprite;

        private void Start()
        {
            Initialization();
        }

        private void Initialization()
        {
            _cellSprites.Add(CellFigure.none, _blankSprite);
            _cellSprites.Add(CellFigure.p1, _p1Sprite);
            _cellSprites.Add(CellFigure.p2, _p2Sprite);

            _subStateSprites.Add(CellSubState.freeze, _freezeSprite);
            _subStateSprites.Add(CellSubState.none, _blankSprite);
        }

        public Sprite GetSprite(CellFigure i)
        {
            return _cellSprites[i];
        }
        
        public Sprite GetSprite(CellSubState i)
        {
            return _subStateSprites[i];
        }
    }
}