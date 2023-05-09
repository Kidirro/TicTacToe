using System.Collections.Generic;
using Cards.CustomType;

namespace UIElements.Interfaces
{
    public interface IBuyingAnimationController
    {
        public void ShowBuyingAnimationWithReset(List<CardInfo> cardInfos);
        public void ShowBuyingAnimation(CardInfo cardInfos);
    }
}