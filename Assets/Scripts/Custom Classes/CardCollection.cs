using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class CardCollection : MonoBehaviour
{
    /// <summary>
    /// ������� �� ����� �� ���������
    /// </summary>
     public bool IsUnlock => Info!=null && (Info.IsDefaultUnlock || PlayerPrefs.GetInt("IsCard"+Info.name+"Unlocked",0)==1);

    /// <summary>
    /// ���������� ����� ��� �������������� ��������
    /// </summary>
    public CardInfo Info;

    /// <summary>
    /// ����� ���������
    /// </summary>
    [Header("Texts"),SerializeField]
    private TextMeshProUGUI _manapoints;

    /// <summary>
    /// ����� �������� �����
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI _cardDescription;

    /// <summary>
    /// ������ �����
    /// </summary>
    [Header("Objects"), SerializeField]
    private GameObject _cardObj;

    [SerializeField]
    private CanvasGroup _canvasGroup;

    /// <summary>
    /// ������ ����� (�������� ���������)
    /// </summary>
    [SerializeField]
    private GameObject _cardObjClose;

    /// <summary>
    /// ����������� �����
    /// </summary>
    [SerializeField]
    private List<GameObject> _bonusImageList = new List<GameObject>();

    /// <summary>
    /// ����������� �����
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

            string desc = "";
            desc= I2.Loc.LocalizationManager.TryGetTranslation(Info.CardDescription, out desc) ? I2.Loc.LocalizationManager.GetTranslation(Info.CardDescription) : Info.CardDescription;
            _cardDescription.text = desc;

            for (int i = 0; i < _bonusImageList.Count; i++)
            {
                _bonusImageList[i].SetActive(i== (int)Info.CardBonus);
            }
        }
    }

    public void SetDeckState(bool state)
    {
        _canvasGroup.alpha = (state) ? 1 : 0.2f;
    }

    public void UnlockCard()
    {
        PlayerPrefs.SetInt("IsCard" + Info.name + "Unlocked", 1);
        UpdateUI();
    }   
    
    public void PickCard()
    {
        Managers.CollectionManager.PickCard(Info);
        SetDeckState(Managers.CollectionManager.IsOnRedactedDeck(Info));
    }

    public void OnPointerDown()
    {
        Managers.CollectionManager.Instance.StartTap(this);
    }

    public void OnPointerUp() 
    {
        Managers.CollectionManager.Instance.EndTap(this);
    }

}
