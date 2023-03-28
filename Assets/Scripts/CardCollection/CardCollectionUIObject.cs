using System.Collections.Generic;
using Cards;
using Cards.CustomType;
using Cards.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CardCollection
{
    public class CardCollectionUIObject : MonoBehaviour
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

        [SerializeField]
        private CanvasGroup _canvasGroup;

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


        #region Dependency

        private ICollectionUIService _collectionUIService;
        
       [Inject]
        private void Construct(ICollectionUIService collectionUIService)
        {
            _collectionUIService = collectionUIService;
        }

        #endregion
 
        
        
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

                string desc;
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
            CollectionManager.PickCard(Info);
            SetDeckState(CollectionManager.IsOnRedactedDeck(Info));
        }

        public void OnPointerDown()
        {
            _collectionUIService.StartTap(this);
        }

        public void OnPointerUp() 
        {
            _collectionUIService.EndTap(this);
        }
        
        

    }
}
