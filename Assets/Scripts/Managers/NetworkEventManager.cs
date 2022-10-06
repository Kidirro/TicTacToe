using ExitGames.Client.Photon;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Managers;
using System;

public class NetworkEventManager : Singleton<NetworkEventManager>, IOnEventCallback
{

    #region Unity

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }


    private void Awake()
    {

        PhotonPeer.RegisterType(typeof(Vector2Int), 200, Converter.SerializeVector2Int, Converter.DeserializeVector2Int);
        PhotonPeer.RegisterType(typeof(Vector3Int), 201, Converter.SerializeVector3Int, Converter.DeserializeVector3Int);

    }

    #endregion

    #region Check_Event

    public static void RaiseEventMasterChecker()
    {
        if (!GameplayManager.IsOnline) return;
        RaiseEventOptions RO = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions SO = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(10, null, RO, SO);
    }

    public static void RaiseEventEndTurn()
    {
        if (!GameplayManager.IsOnline) return;

        RaiseEventOptions RO = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions SO = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(11, null, RO, SO);
    }

    public static void RaiseEventAwaitTime(float time)
    {
        if (!GameplayManager.IsOnline) return;

        RaiseEventOptions RO = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions SO = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(12, time, RO, SO);
    }

    #endregion

    #region Mana_Event
    public static void RaiseEventIncreaseMana(int value, bool isOverMax = false)
    {
        if (!GameplayManager.IsOnline) return;
        RaiseEventOptions RO = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions SO = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(20, new Vector2Int(value, isOverMax ? 1 : 0), RO, SO);
    }

    public static void RaiseEventAddBonusMana(int value)
    {
        if (!GameplayManager.IsOnline) return;
        RaiseEventOptions RO = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions SO = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(21, value, RO, SO);
    }

    public static void RaiseEventIncreaseMaxMana(int value)
    {
        if (!GameplayManager.IsOnline) return;
        RaiseEventOptions RO = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions SO = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(22, value, RO, SO);
    }


    /// <summary>
    /// при -1 AllMana
    /// </summary>
    /// <param name="value"></param>
    public static void RaiseEventRestoreMana(int value)
    {
        if (!GameplayManager.IsOnline) return;
        RaiseEventOptions RO = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions SO = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(23, value, RO, SO);
    }

    #endregion

    #region Card_Event

    public static void RaiseEventCardInvoke(CardInfo card)
    {
        if (!GameplayManager.IsOnline) return;
        RaiseEventOptions RO = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions SO = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(30, card.CardId, RO, SO);
    }

    #endregion

    #region Figure_Event

    public static void RaiseEventPlaceInCell(Vector2Int id)
    {
        if (!GameplayManager.IsOnline) return;
        RaiseEventOptions RO = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions SO = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(40, id, RO, SO);
    }


    #endregion

    #region Freeze_event

    public static void RaiseEventFreezeCell(Vector2Int id)
    {
        if (!GameplayManager.IsOnline) return;

        RaiseEventOptions RO = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions SO = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(50, id, RO, SO);
    }

    public static void RaiseEventUnFreezeCell(Vector2Int id)
    {
        if (!GameplayManager.IsOnline) return;

        RaiseEventOptions RO = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions SO = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(51, id, RO, SO);
    }

    #endregion

    #region Effect_event

    public static void RaiseEventAddEffect(Action action)
    {
        if (!GameplayManager.IsOnline) return;
        if (EffectManager.Instance.GetIdSerializibleAction(action) == -1) return;
        Debug.Log($"Send effect! {EffectManager.Instance.GetIdSerializibleAction(action)}");
        RaiseEventOptions RO = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions SO = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(60, EffectManager.Instance.GetIdSerializibleAction(action), RO, SO);
    }

    public static void RaiseEventAddEffect(int action)
    {
        if (!GameplayManager.IsOnline) return;
        RaiseEventOptions RO = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions SO = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(60, action, RO, SO);
    }

    public static void RaiseEventClearEffect(int action)
    {
        if (!GameplayManager.IsOnline) return;
        RaiseEventOptions RO = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions SO = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(61, action, RO, SO);
    }

    public static void RaiseEventUpdateEffect(int action, int value)
    {
        if (!GameplayManager.IsOnline) return;
        Vector2Int data = new Vector2Int(action, value);
        RaiseEventOptions RO = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions SO = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(62, data, RO, SO);
    }

    public static void RaiseEventAddFreezeEffect(Vector2Int position)
    {
        if (!GameplayManager.IsOnline) return;
        RaiseEventOptions RO = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions SO = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(63, position, RO, SO);
    }

    #endregion

    /*   public static void RaiseEventCellState(Vector2Int id)
       {
           if (!GameplayManager.IsOnline) return;
           RaiseEventOptions RO = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
           SendOptions SO = new SendOptions { Reliability = true };
           PhotonNetwork.RaiseEvent(11, id, RO, SO);
       }

       public static void RaiseEventCardInvoke(CardInfo card)
       {
           if (!GameplayManager.IsOnline) return;
           RaiseEventOptions RO = new RaiseEventOptions { Receivers = ReceiverGroup.All };
           SendOptions SO = new SendOptions { Reliability = true };
           PhotonNetwork.RaiseEvent(4, card.CardId, RO, SO);
       }*/

    void IOnEventCallback.OnEvent(EventData photonEvent)
    {
        Debug.Log("Code:" + photonEvent.Code);
        switch (photonEvent.Code)
        {
            case 10:
                FinishLineManager.Instance.MasterChecker(PlayerManager.Instance.GetCurrentPlayer().SideId);
                break;
            case 11:
                GameplayManager.Instance.SetGamePlayStateQueue(GameplayManager.GameplayState.NewTurn);
                break;
            case 12:
                float data_12 = (float)photonEvent.CustomData;
                CoroutineManager.Instance.AddAwaitTime(data_12);
                break;
            case 20:
                Vector2Int data_20 = (Vector2Int)photonEvent.CustomData;
                ManaManager.Instance.IncreaseMana(data_20.x, data_20.y == 1);
                ManaManager.Instance.UpdateManaUI();
                break;
            case 21:
                ManaManager.Instance.AddBonusMana((int)photonEvent.CustomData);
                ManaManager.Instance.RestoreAllMana();
                ManaManager.Instance.UpdateManaUI();
                break;
            case 22:
                ManaManager.Instance.IncreaseMaxMana((int)photonEvent.CustomData);
                ManaManager.Instance.UpdateManaUI();
                break;
            case 23:
                int data_23 = (int)photonEvent.CustomData;
                if (data_23 == -1) ManaManager.Instance.RestoreAllMana();
                else ManaManager.Instance.RestoreMana(data_23);
                ManaManager.Instance.UpdateManaUI();
                break;
            case 30:
                CardInfo data_30 = CollectionManager.GetCardFromId((int)photonEvent.CustomData);
                HistoryManager.Instance.AddHistoryCard(PlayerManager.Instance.GetCurrentPlayer(), data_30);
                break;
            case 40:
                Vector2Int data_40 = (Vector2Int)photonEvent.CustomData;
                Field.Instance.PlaceInCell(data_40, false);
                break;
            case 50:
                Vector2Int data_50 = (Vector2Int)photonEvent.CustomData;
                Field.Instance.FreezeCell(data_50, false);
                break;
            case 51:
                Vector2Int data_51 = (Vector2Int)photonEvent.CustomData;
                Field.Instance.ResetSubStateWithPlaceFigure(data_51, false);
                break;
            case 60:
                int data_60 = (int)photonEvent.CustomData;
                EffectManager.Instance.AddEffect(data_60);
                break;
            case 61:
                int data_61 = (int)photonEvent.CustomData;
                EffectManager.Instance.ClearEffect(data_61);
                break;
            case 62:
                Vector2Int data_62 = (Vector2Int)photonEvent.CustomData;
                EffectManager.Instance.UpdateEffectState(data_62.x, data_62.y);
                break;
            case 63:
                Vector2Int data_63 = (Vector2Int)photonEvent.CustomData;
                EffectManager.Instance.FreezeCell_Effect(data_63);
                break;
        }


    }
}

/*
 
1* - ивенты со стейт машиной
10 - глобальная проверка    
11 - новый ход
12 - ожидание времени в очереди

2* - ивенты с маной
20 - изменение текущей маны
21 - изменение бонусной маны
22 - изменение максимальной маны
23 - восстановление маны

3* - ивенты с картой
30 - карта разыграна

4* - ивенты с расположением фигуры
40 - расположение фигуры

5* - ивенты с заморозкой
50 - заморозка клетки
51 - разморозка клетки

6* - ивенты с эффектами
60 - добавить сериализуемый эффект 
61 - удалить эффект
62 - обновить количество ходов эффекта
63 - добавить эффект заморозки
64 - активировать OnDisableEffect
 */
