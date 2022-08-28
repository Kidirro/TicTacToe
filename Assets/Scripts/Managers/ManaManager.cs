using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaManager : Singleton<ManaManager>
{

    [SerializeField]
    private int _startManapool;

    private  bool _isManaGrow = true;


    private int _maxManaGrow = 5;

    private int _manapool =1;

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

    public void ResetCurrentMana()
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


}
