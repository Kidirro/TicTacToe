using Tutorial.Interfaces;
using UnityEngine;

namespace Tutorial
{
    public class TutorialCompleteCompleteController : ITutorialCompleteService
    {
        private bool _isTutorialShowed;

        private TutorialCompleteCompleteController()
        {
            _isTutorialShowed = PlayerPrefs.GetInt("IsTutorialShowed", 0) == 1;
        }

        public bool GetIsTutorialComplete()
        {
            return _isTutorialShowed;
        }

        public void SetIsTutorialComplete(bool state)
        {
            _isTutorialShowed = state;
            PlayerPrefs.SetInt("IsTutorialShowed", _isTutorialShowed ? 1 : 0);
        }
    }
}