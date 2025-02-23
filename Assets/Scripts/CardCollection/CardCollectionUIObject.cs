using Cards.CustomType;
using Cards.Interfaces;
using UIElements;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace CardCollection
{
    public class CardCollectionUIObject : CardViewBase, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        /// <summary>
        /// Информация карты как информационной сущности
        /// </summary>
        public CardInfo Info;

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

        public void UpdateUI(bool isUnlock)
        {

            _cardObj.SetActive(isUnlock);
            _cardObjClose.SetActive(!isUnlock);
            if (isUnlock)
            {            
              base.UpdateCardViewImage(Info,new PlayerInfo(){SideId=1});
            }
        }

        public void SetDeckState(bool state)
        {
            _canvasGroup.alpha = (state) ? 1 : 0.2f;
        }
    
        public void OnDrag(PointerEventData eventData)
        {
            _collectionUIService.OnDrag(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _collectionUIService.StartTap(this,eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            
            _collectionUIService.EndTap(this,eventData);
        }
    }
}
