using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public static class AnalitycManager
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