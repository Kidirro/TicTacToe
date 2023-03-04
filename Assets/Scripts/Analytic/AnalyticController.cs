using System;
using System.Collections.Generic;
using Cards;
using GameState;
using Managers;
using UnityEngine;
using Object = System.Object;

namespace Analytic
{
    public static class AnalyticController
    {
        public static void Player_Start_Match(GameplayManager.GameType gameType, List<CardInfo> cardList)
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

        public static void Player_Win_Match(GameplayManager.GameType gameType, List<CardInfo> cardList)
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

        public static void Player_Lose_Match(GameplayManager.GameType gameType, List<CardInfo> cardList)
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

        public static void Player_Leave_Match(GameplayManager.GameType gameType, List<CardInfo> cardList)
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

        public static void Player_Draw_Match(GameplayManager.GameType gameType, List<CardInfo> cardList)
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

        public static void Player_Open_Collection()
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

        public static void Player_Open_Deckbuild()
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

        public static void Player_Open_Store()
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

        public static void Player_Try_Purchase(string budleId)
        {
            try
            {
                Debug.Log(
                    $"Player_Try_Purchase. Bundle {budleId}");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            try
            {
                var dict = new Dictionary<string, object>();
                dict["Bundle"] = budleId;
                AppMetrica.Instance.ReportEvent("Player_Try_Purchase", dict);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        public static void Player_Bought_Bundle(string budleId)
        {
            try
            {
                Debug.Log(
                    $"Player_Bought_Bundle. Bundle {budleId}");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            try
            {
                var dict = new Dictionary<string, object>();
                dict["Bundle"] = budleId;
                AppMetrica.Instance.ReportEvent("Player_Bought_Bundle", dict);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        public static void Player_Try_Watch_Add()
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

        public static void Player_Watched_Add()
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

        public static void Player_Try_Find_Match()
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

        public static void Player_Found_Match(float time)
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

        public static void Player_Cancel_Find_Match(float time)
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

        public static void Player_Bought_Random_Card()
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

        public static void Player_Start_Tutorial()
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

        public static void Player_Complete_Tutorial()
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