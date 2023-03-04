using System.Collections;
using GameState;
using Network;
using TurnTimer.Interfaces;
using UnityEngine;

namespace TurnTimer
{

    public class TurnTimerController : MonoBehaviour, ITurnTimerService
    {
        private const float PLAYER_TURN_TIMssE = 30f;
        private const float BOT_TURN_TIME = 10f;

        private float _timeLeft = 0f;

        private IEnumerator _timerCoroutine;

        public float GetTimeLeft()
        {
            return _timeLeft;
        }

        public void StartNewTurnTimer(float TurnTime)
        {


            StopCoroutine(_timerCoroutine);
            _timeLeft = TurnTime;
            StartCoroutine(_timerCoroutine);

        }
        public void StartNewTurnTimer(PlayerType player, bool isNeedAddToQueue = true)
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
            _timerCoroutine = ITurnTimer(isNeedAddToQueue);
            StartCoroutine(_timerCoroutine);

        }

        private IEnumerator ITurnTimer(bool isNeedAddtoQueue)
        {
            while (_timeLeft > 0)
            {
                yield return null;
                _timeLeft -= Time.deltaTime;
            }
            /*GameplayManager.Instance.SetGameplayState(GameplayState.NewTurn);*/
            if (isNeedAddtoQueue)
            {
                GameplayManager.Instance.SetGamePlayStateQueue(GameplayManager.GameplayState.NewTurn);
                NetworkEventManager.RaiseEventEndTurn();

            }
        }

        public void StopTimer()
        {
            if (_timerCoroutine != null) StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
        }
    }
}