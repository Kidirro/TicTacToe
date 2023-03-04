using UnityEngine;

namespace Theme.Interfaces
{
    public interface IThemeService
    {
        public Sprite GetSprite(CellFigure i);
        public Sprite GetSprite(CellSubState i);
    }
}