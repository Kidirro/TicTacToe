﻿using Cards.CustomType;

namespace Cards.Interfaces
{
    public interface ICollectionService
    {
        public void UnlockRandomCard(bool isNeedUsekCoin = true);

        public void UnlockAllCard();

        public bool IsCardUnlock(CardInfo cardInfo);
    }
}