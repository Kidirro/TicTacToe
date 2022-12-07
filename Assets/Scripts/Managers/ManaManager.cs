using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Managers
{

    public class ManaManager : Singleton<ManaManager>
    {
        private bool _isManaGrow = true;

        [SerializeField]
        private int _startManapool;

        private const int GROW_PER_ROUND = 1;

        private int _manapool = 1;

        private int _bonusMana;

        /// <summary>
        /// 
        /// </summary>
        private int _currentMana;

        public int CurrentMana
        {
            get => _currentMana;
        }

        [SerializeField]
        private TextMeshProUGUI _manaText;

        /// <summary>
        /// 
        /// </summary>
        [Header("Mana colors properties"), SerializeField]
        private List<Image> _manaPointsStroke;

        /// <summary>
        /// 
        /// </summary>
        [SerializeField]
        private List<Image> _manaPointsFill;              

        public bool IsEnoughMana(int mana)
        {
            return mana <= Mathf.Max(0, _currentMana);
        }

        public void IncreaseMaxMana(int mana)
        {
            _manapool += mana;
        }

        public void IncreaseMana(int mana, bool isOverMax = false)
        {
            if (_currentMana + mana > _manapool + _bonusMana)
            {
                int diff = _currentMana + mana - _manapool - _bonusMana;
                _currentMana += mana;
                if (isOverMax)
                {
                    AddBonusMana(diff);
                }
                else
                {
                    _currentMana -= diff;
                }
            }
            else
                _currentMana = _currentMana + mana;
        }

        public void RestoreAllMana()
        {

            _currentMana = _manapool + _bonusMana;
        }

        public void ResetMana(int round = 0)
        {
            _manapool = _startManapool + round * GROW_PER_ROUND;
            _bonusMana = 0;
        }

        public void UpdateManaUI()
        {
            _manaText.text = $"{_currentMana}/{_manapool + _bonusMana}";
            for (int i = 0; i < _manaPointsFill.Count; i++)
            {
                StartCoroutine(_manaPointsFill[i].AlphaWithLerp(_manaPointsFill[i].color.a, (i + 1 <= _currentMana) ? 1 : 0, 20));
                StartCoroutine(_manaPointsStroke[i].AlphaWithLerp(_manaPointsStroke[i].color.a, (i + 1 <= _manapool + _bonusMana) ? 1 : 0, 20));
            }
        }

        public void SetBonusMana(int mana)
        {
            _bonusMana = mana;
        }

        public void AddBonusMana(int mana)
        {
            _bonusMana += mana;
        }

        public void RestoreMana(int value)
        {
            _currentMana = Mathf.Min(_currentMana + value, _manapool + _bonusMana);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S)) RestoreMana(1);
        }


    }
}