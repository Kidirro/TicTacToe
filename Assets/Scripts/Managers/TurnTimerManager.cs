using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{

    public class TurnTimerManager : Singleton<TurnTimerManager>
    {
        public const float PlayerTurnTime = 60F;
        public const float BotTurnTime = 1f;

        private float _timeLeft = 0f;

        private IEnumerator _timerCoroutine;

        private void Awake()
        {
            _timerCoroutine = ITurnTimer();
        }

        public float TimeLeft
        {
            get { return _timeLeft; }
        }

        public void StartNewTurnTimer(float TurnTime)
        {


            StopCoroutine(_timerCoroutine);
            _timeLeft = TurnTime;
            StartCoroutine(_timerCoroutine);

        }
        public void StartNewTurnTimer(PlayerType player)
        {

            Debug.Log("Started timer");
            StopCoroutine(_timerCoroutine);
            switch (player)
            {
                case PlayerType.AI:
                    _timeLeft = BotTurnTime;
                    break;
                case PlayerType.Human:
                    _timeLeft = PlayerTurnTime;
                    break;
            }
            _timerCoroutine = ITurnTimer();
            StartCoroutine(_timerCoroutine);

        }

        private IEnumerator ITurnTimer()
        {
            while (_timeLeft > 0)
            {
                yield return new WaitForSecondsRealtime(0.1f);
                _timeLeft -= 0.1f;
            }
            /*GameplayManager.Instance.SetGameplayState(GameplayState.NewTurn);*/
            GameplayManager.Instance.SetGamePlayStateQueue(GameplayManager.GameplayState.NewTurn);
        }
    }
}