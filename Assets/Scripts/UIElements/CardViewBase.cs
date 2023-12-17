using System.Collections.Generic;
using Cards.CustomType;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIElements
{
    public class CardViewBase: MonoBehaviour
    {
        [Header("Card view"), SerializeField]
        protected CanvasGroup _viewCardCanvas;

        [SerializeField]
        protected TextMeshProUGUI _viewManapoints;

        [SerializeField]
        protected TextMeshProUGUI _viewCardDescription;

        [SerializeField]
        protected Image _viewCardImage;

        [SerializeField]
        protected List<CardView.BonusImageType> _viewBonusImageList = new();

        protected void UpdateCardViewImage(CardInfo info, PlayerInfo player)
        {
            string desc = "";
            desc = I2.Loc.LocalizationManager.TryGetTranslation(info.CardDescription, out desc)
                ? I2.Loc.LocalizationManager.GetTranslation(info.CardDescription)
                : info.CardDescription;
            _viewCardDescription.text = desc;
            _viewCardImage.sprite = (player.SideId == 1) ? info.CardImageP1 : info.CardImageP2;
            _viewManapoints.text = info.CardManacost.ToString();
            foreach (CardView.BonusImageType bonus in _viewBonusImageList)
            {
                bonus.BonusImage.SetActive(bonus.BonusType == info.CardBonus);
            }
        }
    }
}