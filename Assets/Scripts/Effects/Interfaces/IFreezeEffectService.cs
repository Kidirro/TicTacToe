using System.Collections.Generic;

namespace Effects.Interfaces
{
    public interface IFreezeEffectService
    {
        public List<Cell> GetFreezeCell();
        public List<Effect> GetFreezeEffect();
        public void ReleaseFreeze(Cell cell);
        public void ReleaseFreeze(Effect effect);
    }
}