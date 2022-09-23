using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CardCollection : MonoBehaviour
{

    /// <summary>
    /// Открыта ли карта по умолчанию
    /// </summary>
     public bool IsUnlock => Info!=null && (Info.IsDefaultUnlock || PlayerPrefs.GetInt("IsCard"+Info.name+"Unlocked",0)==1);

    /// <summary>
    /// Информация карты как информационной сущности
    /// </summary>
    public CardInfo Info;

    /// <summary>
    /// Текст манакоста
    /// </summary>
    [Header("Texts"),SerializeField]
    private TextMeshProUGUI _manapoints;

    /// <summary>
    /// Текст описания карты
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI _cardDescription;

    /// <summary>
    /// Обьект карты
    /// </summary>
    [Header("Objects"), SerializeField]
    private GameObject _cardObj;

    /// <summary>
    /// Обьект карты (закрытое состояние)
    /// </summary>
    [SerializeField]
    private GameObject _cardObjClose;

    /// <summary>
    /// Изображение карты
    /// </summary>
    [SerializeField]
    private List<GameObject> _bonusImageList = new List<GameObject>();

    /// <summary>
    /// Изображение карты
    /// </summary>
    [SerializeField]
    private Image _cardImage;

    private void Awake()
    {
        if (Info == null) Destroy(this.gameObject);
    }

    public void UpdateUI()
    {

        _cardObj.SetActive(IsUnlock);
        _cardObjClose.SetActive(!IsUnlock);
        if (IsUnlock)
        {            
            _manapoints.text = (Info.CardManacost + Info.CardBonusManacost).ToString();
            _cardImage.sprite = Info.CardImageP1;
            _cardDescription.text = Info.CardDescription;
            for (int i = 0; i < _bonusImageList.Count; i++)
            {
                _bonusImageList[i].SetActive(i + 1 == (int)Info.CardBonus);
            }
        }
    }

    public void RewerseState()
    {
            PlayerPrefs.SetInt("IsCard" + Info.name + "Unlocked", IsUnlock?0:1);
        UpdateUI();
    }

    public void UnlockCard()
    {
        PlayerPrefs.SetInt("IsCard" + Info.name + "Unlocked", 1);
        UpdateUI();
    }
}
