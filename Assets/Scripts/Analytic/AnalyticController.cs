using System;
using System.Collections.Generic;
using Analytic.Interfaces;
using Cards;
using Cards.CustomType;
using GameTypeService.Enums;
using UnityEngine;
using Object = System.Object;

namespace Analytic
{
    public class AnalyticController : IMatchEventsAnalyticService, ICollectionEventsAnalyticService,
        IStoreEventsAnalyticService, IAdEventsAnalyticService, ITutorialEventsEventsAnalyticService
    {
        #region MatchEvents

        public void Player_Start_Match(GameType gameType, List<CardInfo> cardList)
        {
            try
            {
                Debug.Log(
                    $"Player_Start_Match. GameType {gameType.ToString()}. CardCount {cardList.Count}. {GenerateCardString(cardList)}");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            try
            {
                var dict = GenerateCardParams(cardList);
                dict["CardCount"] = cardList.Count;
                dict["GameType"] = gameType.ToString();
                AppMetrica.Instance.ReportEvent("Player_Start_Match", dict);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        public void Player_Win_Match(GameType gameType, List<CardInfo> cardList)
        {
            try
            {
                Debug.Log(
                    $"Player_Win_Match. GameType {gameType.ToString()}. CardCount {cardList.Count}. {GenerateCardString(cardList)}");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            try
            {
                var dict = GenerateCardParams(cardList);
                dict["CardCount"] = cardList.Count;
                dict["GameType"] = gameType.ToString();
                AppMetrica.Instance.ReportEvent("Player_Win_Match", dict);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        public void Player_Lose_Match(GameType gameType, List<CardInfo> cardList)
        {
            try
            {
                Debug.Log(
                    $"Player_Lose_Match. GameType {gameType.ToString()}. CardCount {cardList.Count}. {GenerateCardString(cardList)}");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            try
            {
                var dict = GenerateCardParams(cardList);
                dict["CardCount"] = cardList.Count;
                dict["GameType"] = gameType.ToString();
                AppMetrica.Instance.ReportEvent("Player_Lose_Match", dict);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        public void Player_Leave_Match(GameType gameType, List<CardInfo> cardList)
        {
            try
            {
                Debug.Log(
                    $"Player_Leave_Match. GameType {gameType.ToString()}. CardCount {cardList.Count}. {GenerateCardString(cardList)}");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            try
            {
                var dict = GenerateCardParams(cardList);
                dict["CardCount"] = cardList.Count;
                dict["GameType"] = gameType.ToString();
                AppMetrica.Instance.ReportEvent("Player_Leave_Match", dict);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        public void Player_Draw_Match(GameType gameType, List<CardInfo> cardList)
        {
            try
            {
                Debug.Log(
                    $"Player_Draw_Match. GameType {gameType.ToString()}. CardCount {cardList.Count}. {GenerateCardString(cardList)}");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            try
            {
                var dict = GenerateCardParams(cardList);
                dict["CardCount"] = cardList.Count;
                dict["GameType"] = gameType.ToString();
                AppMetrica.Instance.ReportEvent("Player_Draw_Match", dict);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        public void Player_Try_Find_Match()
        {
            try
            {
                Debug.Log(
                    $"Player_Try_Find_Match");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            try
            {
                AppMetrica.Instance.ReportEvent("Player_Try_Find_Match");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        public void Player_Found_Match(float time)
        {
            try
            {
                Debug.Log(
                    $"Player_Found_Match. Time : {time}");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            try
            {
                var dict = new Dictionary<string, Object>();
                dict["Time"] = time;
                AppMetrica.Instance.ReportEvent("Player_Try_Find_Match", dict);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        public void Player_Cancel_Find_Match(float time)
        {
            try
            {
                Debug.Log(
                    $"Player_Cancel_Find_Match. Time : {time}");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            try
            {
                var dict = new Dictionary<string, Object>();
                dict["Time"] = time;
                AppMetrica.Instance.ReportEvent("Player_Cancel_Find_Match", dict);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        #endregion

        #region CollectionEvents

        public void Player_Open_Collection()
        {
            try
            {
                Debug.Log(
                    $"Player_Open_Collection");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            try
            {
                AppMetrica.Instance.ReportEvent("Player_Open_Collection");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        public void Player_Open_Deckbuild()
        {
            try
            {
                Debug.Log(
                    $"Player_Open_Deckbuild");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            try
            {
                AppMetrica.Instance.ReportEvent("Player_Open_Deckbuild");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        #endregion

        #region SoreEvents

        public void Player_Open_Store()
        {
            try
            {
                Debug.Log(
                    $"Player_Open_Store");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            try
            {
                AppMetrica.Instance.ReportEvent("Player_Open_Store");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        public void Player_Try_Purchase(string bundleId)
        {
            try
            {
                Debug.Log(
                    $"Player_Try_Purchase. Bundle {bundleId}");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            try
            {
                var dict = new Dictionary<string, object>();
                dict["Bundle"] = bundleId;
                AppMetrica.Instance.ReportEvent("Player_Try_Purchase", dict);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        public void Player_Bought_Bundle(string bundleId)
        {
            try
            {
                Debug.Log(
                    $"Player_Bought_Bundle. Bundle {bundleId}");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            try
            {
                var dict = new Dictionary<string, object>();
                dict["Bundle"] = bundleId;
                AppMetrica.Instance.ReportEvent("Player_Bought_Bundle", dict);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        public void Player_Bought_Random_Card()
        {
            try
            {
                Debug.Log(
                    $"Player_Bought_Random_Card");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            try
            {
                AppMetrica.Instance.ReportEvent("Player_Bought_Random_Card");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        #endregion

        #region AdEvents

        public void Player_Try_Watch_Add()
        {
            try
            {
                Debug.Log(
                    $"Player_Try_Watch_Add");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            try
            {
                AppMetrica.Instance.ReportEvent("Player_Try_Watch_Add");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        public void Player_Watched_Add()
        {
            try
            {
                Debug.Log(
                    $"Player_Watched_Add");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            try
            {
                AppMetrica.Instance.ReportEvent("Player_Watched_Add");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        #endregion

        #region TutorialEvents

        public void Player_Start_Tutorial()
        {
            try
            {
                Debug.Log(
                    $"Player_Start_Tutorial");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            try
            {
                AppMetrica.Instance.ReportEvent("Player_Start_Tutorial");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        public void Player_Complete_Tutorial()
        {
            try
            {
                Debug.Log(
                    $"Player_Complete_Tutorial");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            try
            {
                AppMetrica.Instance.ReportEvent("Player_Complete_Tutorial");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        #endregion
        
        private static Dictionary<string, object> GenerateCardParams(List<CardInfo> cardList)
        {
            var dict = new Dictionary<string, object>();

            for (int i = 0; i < cardList.Count; i++)
            {
                dict[$"Card {i + 1}"] = cardList[i].CardName;
            }

            return dict;
        }

        private static string GenerateCardString(List<CardInfo> cardList)
        {
            string result = "";

            for (int i = 0; i < cardList.Count; i++)
            {
                result += $"Card {i + 1}: {cardList[i].CardName};";
            }

            return result;
        }
    }
}