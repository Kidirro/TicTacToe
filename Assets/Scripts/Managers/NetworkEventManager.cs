using ExitGames.Client.Photon;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Managers;

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

    public static void RaiseEventSetSubState(Vector2Int id, CardInfo card)
    {
        if (!GameplayManager.IsOnline) return;

        Vector3Int data = new Vector3Int(id.x, id.y, card.CardId);

        RaiseEventOptions RO = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions SO = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(50, data, RO, SO);
    }
    
    public static void RaiseEventSetSubState(Vector2Int id, int card)
    {
        if (!GameplayManager.IsOnline) return;

        Vector3Int data = new Vector3Int(id.x, id.y, card);

        RaiseEventOptions RO = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions SO = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(50, data, RO, SO);
    }

    public static void RaiseEventResetSubState(Vector2Int id)
    {
        if (!GameplayManager.IsOnline) return;


        RaiseEventOptions RO = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions SO = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(51, id, RO, SO);
    }    
    
   /* public static void RaiseEventFreezeCell(EffectTest id)
    {
        if (!GameplayManager.IsOnline) return;

        Debug.Log($"effect send! {id.EffectType} : {id.EffectTurnCount}");
        RaiseEventOptions RO = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions SO = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(52, id, RO, SO);
    }
*/
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
                Field.Instance.PlaceInCell(data_40);
                break;
            case 50:
                Vector3Int data_50 = (Vector3Int)photonEvent.CustomData;
                Vector2Int position = new Vector2Int(data_50.x, data_50.y);
                CardInfo card = CollectionManager.GetCardFromId(data_50.z);
               /* Field.Instance.FreezeCellDisableEffect(
                               position,
                               PlayerManager.Instance.GetCurrentPlayer().SideId == 1 ? card.CardHighlightP1 : card.CardHighlightP2);*/
                break;
            case 51:
                Vector2Int data_51 = (Vector2Int)photonEvent.CustomData;
                Field.Instance.ResetSubStateZone(data_51, Vector2Int.one);
                break;
            case 52:
          /*      EffectTest data_52 = (EffectTest)photonEvent.CustomData;

                Debug.Log($"{data_52.EffectType} : {data_52.EffectTurnCount}");*/

                break;
        }


    }
}

/*
 
1* - ивенты со стейт машиной
10 - глобальная проверка    
11 - новый ход

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
 */
