using Cards.Enum;
using UnityEngine;

namespace Cards.CustomType
{
    [CreateAssetMenu(fileName = "New card", menuName = "Card")]
    public class CardInfo : ScriptableObject
    {
        [HideInInspector]
        public int CardId;

        [Space, Header("Images")]
        public Sprite CardImageP1;

        public Sprite CardImageP2;
        public Sprite CardHighlightP1;
        public Sprite CardHighlightP2;
        public Color CardHighlightColor;

        [Space, Header("Name")]
        public string CardName;

        public string CardDescription;

        [Space, Space, Space]
        public bool IsDefaultUnlock;

        public CardTypeImpact CardType;
        public CardBonusType CardBonus;
        public Vector2Int CardAreaSize;
        public int CardCount;

        public bool IsNeedShowTip;
        public string TipText;

        [Range(0, 5)]
        public int CardManacost;

        [HideInInspector]
        public int CardBonusManacost;

        public string СardActionId;
    }
}