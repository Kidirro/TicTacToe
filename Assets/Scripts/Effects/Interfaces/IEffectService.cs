using System.Collections;
using System.Collections.Generic;

namespace Effects.Interfaces
{
    public interface IEffectService
    {
        public void AddEffect(Effect effect);
        public void AddEffect(int effect);
        public void ClearEffect();
        public void ClearEffect(int id);
        public void UpdateEffectState(int id, int value);
        public IEnumerator UpdateEffectTurn(List<Effect> effects = null);

        public List<Effect> GetEffectList();
    }
}