using System;
using UnityEngine;

namespace Effects.Interfaces
{
    public interface ISerializableEffects
    {
        public int GetIdSerializableAction(Action action);
        public void AddFigure_Effect();
        public void AddBonusMana_Effect();
        public void Decrease2MaxMana_Effect();
        public void Increase2MaxMana_Effect();
        public void Random2Mana_Effect();
        public void DecreaseIncrease2Mana_Effect();
        public void FreezeCell_Effect(Vector2Int id);
        public void Freeze3Cell_Effected();
    }
}