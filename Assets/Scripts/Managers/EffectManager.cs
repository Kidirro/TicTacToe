using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{

    private List<Effect> _effectList = new List<Effect>();

    public void AddEffect(Effect effect)
    {
        _effectList.Add(effect);
    }

    public void UpdateEffectTurn()
    {
        foreach (Effect effect in _effectList)
        {
            Debug.Log("Checked effect");

            if (effect.EffectSideId == PlayerManager.Instance.GetCurrentPlayer().SideId)
            {
                effect.EffectTurnCount -= 1;
                Debug.Log("Effect turn delete");

                effect.EffectAction.Invoke();

                Debug.Log("Effect Invoke");                
            }
        }
        _effectList.RemoveAll(Effect => Effect.EffectTurnCount == 0);
    }
}

public class Effect
{
    public Action EffectAction;
    public int EffectTurnCount;
    public int EffectSideId;

    public Effect(Action action, int turnCount, int SideId)
    {
        EffectAction = action;
        EffectTurnCount = turnCount;
        EffectSideId = SideId;
    }
}