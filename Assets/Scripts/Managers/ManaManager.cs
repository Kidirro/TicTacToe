using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        /// <summary>
        /// 
        /// </summary>
        [SerializeField]
        private List<GameObject> _manaPoints;

        /// <summary>
        /// 
        /// </summary>
        [SerializeField]
        private List<GameObject> _manaPointsFill;

        public bool IsEnoughMana(int mana)
        {
            return mana <= _currentMana;
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

        public void ResetMana(int round =0)
        {
            _manapool = _startManapool+round*GROW_PER_ROUND;
            _bonusMana = 0;
        }

        public void UpdateManaUI()
        {
            for (int i = 0; i < _manaPoints.Count; i++)
            {
                _manaPointsFill[i].SetActive(i + 1 <= _currentMana);
                _manaPoints[i].SetActive(i + 1 <= _manapool + _bonusMana);
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
            Debug.Log(_currentMana);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S)) RestoreMana(1);
        }


    }
}