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

        private void Awake()
        {
            Initialization();
        }

        private void Initialization()
        {
            _cellSprites.Add(CellFigure.None, _blankSprite);
            _cellSprites.Add(CellFigure.P1, _p1Sprite);
            _cellSprites.Add(CellFigure.P2, _p2Sprite);

            _subStateSprites.Add(CellSubState.Freeze, _freezeSprite);
            _subStateSprites.Add(CellSubState.None, _blankSprite);
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