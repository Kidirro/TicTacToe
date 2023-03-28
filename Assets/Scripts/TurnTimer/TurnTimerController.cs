using System;
using System.Collections;
using TurnTimer.Interfaces;
using UnityEngine;

namespace TurnTimer
{

    public class TurnTimerController : MonoBehaviour, ITurnTimerService
    {
        private const float PLAYER_TURN_TIME = 30f;
        private const float BOT_TURN_TIME = 10f;

        private float _timeLeft;

        private IEnumerator _timerCoroutine;

        public float GetTimeLeft()
        {
            return _timeLeft;
        }

        public void StartNewTurnTimer(float turnTime)
        {
            StopCoroutine(_timerCoroutine);
            _timeLeft = turnTime;
            StartCoroutine(_timerCoroutine);

        }
        
        public void StartNewTurnTimer(PlayerType player, Action action=null)
        {

            Debug.Log("Started timer");
            if (_timerCoroutine != null) StopCoroutine(_timerCoroutine);
            switch (player)
            {
                case PlayerType.AI:
                    _timeLeft = BOT_TURN_TIME;
                    break;
                case PlayerType.Human:
                    _timeLeft = PLAYER_TURN_TIME;
                    break;
            }
            _timerCoroutine = ITurnTimer(action);
            StartCoroutine(_timerCoroutine);

        }

        private IEnumerator ITurnTimer(Action action)
        {
            while (_timeLeft > 0)
            {
                yield return null;
                _timeLeft -= Time.deltaTime;
            }
            action?.Invoke();
        }

        public void StopTimer()
        {
            if (_timerCoroutine != null) StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
        }
    }
}