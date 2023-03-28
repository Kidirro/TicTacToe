using System.Collections.Generic;
using Mana.Interfaces;
using TMPro;
using UnityEngine;
using Zenject;

namespace Mana
{
    public class ManaUIController : MonoBehaviour, IManaUIService
    {
        [SerializeField]
        private TextMeshProUGUI _manaText;

        [SerializeField]
        private List<CanvasGroup> _manaPointsGroup;

        private const float ENABLE_ALPHA = 1f;
        private const float DISABLE_ALPHA = 0f;
        private const float USED_ALPHA = 0.2f;

        #region Dependency

        private IManaService _manaService;
        
        [Inject]
        private void Construct(IManaService manaService)
        {
            _manaService = manaService;
        }

        #endregion
 

        public void UpdateManaUI()
        {
            _manaText.text = $"{_manaService.GetCurrentMana()}/{_manaService.GetMaxMana()}";
            for (int i = 0; i < _manaPointsGroup.Count; i++)
            {
                if (i + 1 > _manaService.GetMaxMana())
                    StartCoroutine(_manaPointsGroup[i].AlphaWithLerp(_manaPointsGroup[i].alpha, DISABLE_ALPHA, 20));
                else
                    StartCoroutine(_manaPointsGroup[i]
                        .AlphaWithLerp(_manaPointsGroup[i].alpha, 1 + i <= _manaService.GetCurrentMana() ? ENABLE_ALPHA : USED_ALPHA, 20));
            }
        }
    }
}