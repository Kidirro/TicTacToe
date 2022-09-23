using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{

    public class EffectManager : Singleton<EffectManager>
    {

        public static bool IsEffectManagerDone = true;

        private List<Effect> _effectList = new List<Effect>();
        
        public List<Effect> EffectList
        {
            get => _effectList;
            set => _effectList = value;
        }

        public void AddEffect(Effect effect)
        {
            _effectList.Add(effect);
        }

        public void ClearEffect()
        {
            _effectList.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="effects"></param>
        /// <returns></returns>
        public IEnumerator UpdateEffectTurn(List<Effect> effects = null)
        {
            List<Effect> effectList = effects;
            if (effects == null)
            {
                Debug.Log("Effect: create list");
                effectList = _effectList.FindAll(x => x.EffectSideId == PlayerManager.Instance.GetCurrentPlayer().SideId);
                Debug.Log(PlayerManager.Instance.GetCurrentPlayer().EntityType);
                Debug.Log("Effect: list count" + effectList.Count);
                effectList.Sort(delegate (Effect effect1, Effect effect2)
                {
                    return (effect1.EffectPriority >= effect2.EffectPriority) ? 1 : -1;
                });
            }
            float maxTime = 0;
            if (effectList.Count != 0)
            {
                int lastPriority = effectList[0].EffectPriority;
                while (effectList.Count > 0)
                {
                    Debug.Log("Effect: effect prior" + effectList[0].EffectPriority);
                    bool isAwaitNeed = lastPriority != effectList[0].EffectPriority;
                    lastPriority = effectList[0].EffectPriority;
                    if (isAwaitNeed)
                    {
                        //����������� ��� �������� �� ��������� ������� ����������
                        StartCoroutine(IEffectAwaitAsync(effectList, maxTime));
                        yield break;
                    }
                    Effect effect = effectList[0];
                    ActivateEffect(effect);
                    effectList.Remove(effect);
                    if (effect.EffectType == Effect.EffectTypes.Parallel)
                    {
                        maxTime = Mathf.Max(maxTime, effect.EffectTimeAction, (effect.EffectTurnCount == 0) ? effect.EffectTimeDisable : 0);
                        Debug.Log(maxTime);
                    }
                    else
                    {
                        StartCoroutine(IEffectAwaitAsync(effectList));
                        yield break;
                    }



                }
            }
            _effectList.RemoveAll(x => x.EffectTurnCount == 0);
            Debug.Log("Effect: Begin " + maxTime);
            yield return new WaitForSeconds(maxTime);
            Debug.Log("Effect: Begin Master Checker");
            FinishLineManager.Instance.MasterChecker((CellFigure)PlayerManager.Instance.GetCurrentPlayer().SideId);
            Debug.Log("Effect: End Master Checker");
        }

        private IEnumerator IEffectAwaitAsync(List<Effect> effects, float startAwait = 0)
        {
            yield return new WaitForSeconds(startAwait);
            while (!CoroutineManager.IsQueueEmpty) yield return null;
            CoroutineManager.Instance.AddCoroutine(UpdateEffectTurn(effects));
        }

        private void ActivateEffect(Effect effect)
        {

            effect.EffectTurnCount -= 1;
            Debug.Log("Effect turn delete");


            effect.EffectAction.Invoke();
            if (effect.EffectTurnCount == 0) effect.OnEffectDisable.Invoke();

            Debug.Log("Effect Invoke");

        }
    }

    public class Effect
    {
        public Action EffectAction;
        public Action OnEffectDisable;
        public int EffectTurnCount;
        public int EffectSideId;
        public float EffectTimeAction;
        public float EffectTimeDisable;
        public EffectTypes EffectType;
        public int EffectPriority;


        public enum EffectTypes
        {
            Parallel,
            Consistently
        }

        public Effect(Action action, int turnCount, int SideId, EffectTypes effectType, int priority, Action onDisable = null, float effectTime = 0, float effectTimeDisable = 0)
        {
            if (onDisable == null) onDisable = delegate () { };

            EffectAction = action;
            OnEffectDisable = onDisable;
            EffectTurnCount = turnCount;
            EffectSideId = SideId;

            EffectTimeAction = effectTime;
            EffectTimeDisable = effectTimeDisable;
            EffectType = effectType;
            EffectPriority = priority;
        }
    }
}