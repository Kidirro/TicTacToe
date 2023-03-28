using System.Collections.Generic;
using Cards;
using Cards.CustomType;
using GameTypeService.Enums;

namespace Analytic.Interfaces
{
    public interface IMatchEventsAnalyticService
    {
        public void Player_Start_Match(GameType gameType, List<CardInfo> cardList);
        public void Player_Win_Match(GameType gameType, List<CardInfo> cardList);
        public void Player_Lose_Match(GameType gameType, List<CardInfo> cardList);
        public void Player_Leave_Match(GameType gameType, List<CardInfo> cardList);
        public void Player_Draw_Match(GameType gameType, List<CardInfo> cardList);
        public void Player_Try_Find_Match();
        public void Player_Found_Match(float time);
        public void Player_Cancel_Find_Match(float time);
        
    }
}