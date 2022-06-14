using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaManager : Singleton<ManaManager>
{
    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private int _maxMana;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
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

    public void ResetMana()
    {
        _currentMana = _maxMana + _bonusMana;
    }

    public void UpdateManaUI()
    {
        for (int i = 0; i < _manaPoints.Count; i++)
        {
            _manaPointsFill[i].SetActive(i + 1 <= _currentMana);
            _manaPoints[i].SetActive(i + 1 <= _maxMana + _bonusMana);
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
}
