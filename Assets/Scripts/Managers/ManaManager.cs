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


        [SerializeField]
        private int _maxManaGrow = 5;

        private int _manapool = 1;

        private int _bonusMana;

        /// <summary>
        /// 
        /// </summary>
        private int _currentMana;

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

        public void DecreaseMana(int mana)
        {
            _currentMana -= mana;
        }
                
        public void IncreaseMaxMana(int mana)
        {
            _manapool += mana;
        }

        public void RestoreAllMana()
        {

            _currentMana = _manapool + _bonusMana;
        }

        public void ResetMana()
        {
            _manapool = _startManapool;
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

        public void GrowMana()
        {
            if (_isManaGrow)
            {
                if (_maxManaGrow > _manapool)
                {
                    _manapool += 1;
                }
            }
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