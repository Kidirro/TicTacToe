using System;
using Cards;
using Coroutine;
using Effects.Interfaces;
using ExitGames.Client.Photon;
using GameState;
using History.Interfaces;
using Mana;
using Managers;
using Photon.Pun;
using Photon.Realtime;
using Players.Interfaces;
using UnityEngine;
using Zenject;

namespace Network
{
    public class NetworkEventManager : IOnEventCallback
    {
        #region Interfaces

        private ISerializableEffects _serializableEffects;
        private IEffectService _effectService;
        private IPlayerService _playerService;
        private IHistoryService _historyService;

        #endregion

        [Inject]
        private void Construct(
            ISerializableEffects serializableEffects,
            IEffectService effectService,
            IPlayerService playerService,
            IHistoryService historyService)
        {
            _serializableEffects = serializableEffects;
            _effectService = effectService;
            _playerService = playerService;
            _historyService = historyService;
        }

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
            PhotonPeer.RegisterType(typeof(Vector2Int), 200, Converter.SerializeVector2Int,
                Converter.DeserializeVector2Int);
            PhotonPeer.RegisterType(typeof(Vector3Int), 201, Converter.SerializeVector3Int,
                Converter.DeserializeVector3Int);
        }

        #endregion

        #region Check_Event

        public static void RaiseEventMasterChecker()
        {
            if (!GameplayManager.IsOnline) return;
            RaiseEventOptions ro = new RaiseEventOptions {Receivers = ReceiverGroup.Others};
            SendOptions so = new SendOptions {Reliability = true};
            PhotonNetwork.RaiseEvent(10, null, ro, so);
        }

        public static void RaiseEventEndTurn()
        {
            if (!GameplayManager.IsOnline) return;

            RaiseEventOptions ro = new RaiseEventOptions {Receivers = ReceiverGroup.Others};
            SendOptions so = new SendOptions {Reliability = true};
            PhotonNetwork.RaiseEvent(11, null, ro, so);
        }

        public static void RaiseEventAwaitTime(float time)
        {
            if (!GameplayManager.IsOnline) return;

            RaiseEventOptions ro = new RaiseEventOptions {Receivers = ReceiverGroup.Others};
            SendOptions so = new SendOptions {Reliability = true};
            PhotonNetwork.RaiseEvent(12, time, ro, so);
        }

        #endregion

        #region Mana_Event

        public static void RaiseEventIncreaseMana(int value, bool isOverMax = false)
        {
            if (!GameplayManager.IsOnline) return;
            RaiseEventOptions ro = new RaiseEventOptions {Receivers = ReceiverGroup.Others};
            SendOptions so = new SendOptions {Reliability = true};
            PhotonNetwork.RaiseEvent(20, new Vector2Int(value, isOverMax ? 1 : 0), ro, so);
        }

        public static void RaiseEventAddBonusMana(int value)
        {
            if (!GameplayManager.IsOnline) return;
            RaiseEventOptions ro = new RaiseEventOptions {Receivers = ReceiverGroup.Others};
            SendOptions so = new SendOptions {Reliability = true};
            PhotonNetwork.RaiseEvent(21, value, ro, so);
        }

        public static void RaiseEventIncreaseMaxMana(int value)
        {
            if (!GameplayManager.IsOnline) return;
            RaiseEventOptions ro = new RaiseEventOptions {Receivers = ReceiverGroup.Others};
            SendOptions so = new SendOptions {Reliability = true};
            PhotonNetwork.RaiseEvent(22, value, ro, so);
        }


        /// <summary>
        /// ��� -1 AllMana
        /// </summary>
        /// <param name="value"></param>
        public static void RaiseEventRestoreMana(int value)
        {
            if (!GameplayManager.IsOnline) return;
            RaiseEventOptions ro = new RaiseEventOptions {Receivers = ReceiverGroup.Others};
            SendOptions so = new SendOptions {Reliability = true};
            PhotonNetwork.RaiseEvent(23, value, ro, so);
        }

        #endregion

        #region Card_Event

        public static void RaiseEventCardInvoke(CardInfo card)
        {
            if (!GameplayManager.IsOnline) return;
            RaiseEventOptions ro = new RaiseEventOptions {Receivers = ReceiverGroup.Others};
            SendOptions so = new SendOptions {Reliability = true};
            PhotonNetwork.RaiseEvent(30, card.CardId, ro, so);
        }

        #endregion

        #region Figure_Event

        public static void RaiseEventPlaceInCell(Vector2Int id)
        {
            if (!GameplayManager.IsOnline) return;
            RaiseEventOptions ro = new RaiseEventOptions {Receivers = ReceiverGroup.Others};
            SendOptions so = new SendOptions {Reliability = true};
            PhotonNetwork.RaiseEvent(40, id, ro, so);
        }

        #endregion

        #region Freeze_event

        public static void RaiseEventFreezeCell(Vector2Int id)
        {
            if (!GameplayManager.IsOnline) return;

            RaiseEventOptions ro = new RaiseEventOptions {Receivers = ReceiverGroup.Others};
            SendOptions so = new SendOptions {Reliability = true};
            PhotonNetwork.RaiseEvent(50, id, ro, so);
        }

        public static void RaiseEventUnFreezeCell(Vector2Int id)
        {
            if (!GameplayManager.IsOnline) return;

            RaiseEventOptions ro = new RaiseEventOptions {Receivers = ReceiverGroup.Others};
            SendOptions so = new SendOptions {Reliability = true};
            PhotonNetwork.RaiseEvent(51, id, ro, so);
        }

        #endregion

        #region Effect_event

        public void RaiseEventAddEffect(Action action)
        {
            if (!GameplayManager.IsOnline) return;
            if (_serializableEffects.GetIdSerializableAction(action) == -1) return;
            Debug.Log($"Send effect! {_serializableEffects.GetIdSerializableAction(action)}");
            RaiseEventOptions ro = new RaiseEventOptions {Receivers = ReceiverGroup.Others};
            SendOptions so = new SendOptions {Reliability = true};
            PhotonNetwork.RaiseEvent(60, _serializableEffects.GetIdSerializableAction(action), ro, so);
        }

        public static void RaiseEventAddEffect(int action)
        {
            if (!GameplayManager.IsOnline) return;
            RaiseEventOptions ro = new RaiseEventOptions {Receivers = ReceiverGroup.Others};
            SendOptions so = new SendOptions {Reliability = true};
            PhotonNetwork.RaiseEvent(60, action, ro, so);
        }

        public static void RaiseEventClearEffect(int action)
        {
            if (!GameplayManager.IsOnline) return;
            RaiseEventOptions ro = new RaiseEventOptions {Receivers = ReceiverGroup.Others};
            SendOptions so = new SendOptions {Reliability = true};
            PhotonNetwork.RaiseEvent(61, action, ro, so);
        }

        public static void RaiseEventUpdateEffect(int action, int value)
        {
            if (!GameplayManager.IsOnline) return;
            Vector2Int data = new Vector2Int(action, value);
            RaiseEventOptions ro = new RaiseEventOptions {Receivers = ReceiverGroup.Others};
            SendOptions so = new SendOptions {Reliability = true};
            PhotonNetwork.RaiseEvent(62, data, ro, so);
        }

        public static void RaiseEventAddFreezeEffect(Vector2Int position)
        {
            if (!GameplayManager.IsOnline) return;
            RaiseEventOptions ro = new RaiseEventOptions {Receivers = ReceiverGroup.Others};
            SendOptions so = new SendOptions {Reliability = true};
            PhotonNetwork.RaiseEvent(63, position, ro, so);
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
                    FinishLineManager.Instance.MasterChecker(_playerService.GetCurrentPlayer().SideId,
                        isNeedEvent: false);
                    break;
                case 11:
                    GameplayManager.Instance.SetGamePlayStateQueue(GameplayManager.GameplayState.NewTurn);
                    break;
                case 12:
                    float data12 = (float) photonEvent.CustomData;
                    CoroutineQueueController.Instance.AddAwaitTime(data12);
                    break;
                case 20:
                    Vector2Int data20 = (Vector2Int) photonEvent.CustomData;
                    ManaManager.Instance.IncreaseMana(data20.x, data20.y == 1);
                    ManaManager.Instance.UpdateManaUI();
                    break;
                case 21:
                    ManaManager.Instance.AddBonusMana((int) photonEvent.CustomData);
                    ManaManager.Instance.RestoreAllMana();
                    ManaManager.Instance.UpdateManaUI();
                    break;
                case 22:
                    ManaManager.Instance.IncreaseMaxMana((int) photonEvent.CustomData);
                    ManaManager.Instance.UpdateManaUI();
                    break;
                case 23:
                    int data23 = (int) photonEvent.CustomData;
                    if (data23 == -1) ManaManager.Instance.RestoreAllMana();
                    else ManaManager.Instance.RestoreMana(data23);
                    ManaManager.Instance.UpdateManaUI();
                    break;
                case 30:
                    CardInfo data30 = CollectionManager.GetCardFromId((int) photonEvent.CustomData);
                    _historyService.AddHistoryCard(_playerService.GetCurrentPlayer(), data30);
                    break;
                case 40:
                    Vector2Int data40 = (Vector2Int) photonEvent.CustomData;
                    Field.Instance.PlaceInCell(data40, false);
                    break;
                case 50:
                    Vector2Int data50 = (Vector2Int) photonEvent.CustomData;
                    Field.Instance.FreezeCell(data50, false);
                    break;
                case 51:
                    Vector2Int data51 = (Vector2Int) photonEvent.CustomData;
                    Field.Instance.ResetSubStateWithPlaceFigure(data51, false);
                    break;
                case 60:
                    int data60 = (int) photonEvent.CustomData;
                    _effectService.AddEffect(data60);
                    break;
                case 61:
                    int data61 = (int) photonEvent.CustomData;
                    _effectService.ClearEffect(data61);
                    break;
                case 62:
                    Vector2Int data62 = (Vector2Int) photonEvent.CustomData;
                    _effectService.UpdateEffectState(data62.x, data62.y);
                    break;
                case 63:
                    Vector2Int data63 = (Vector2Int) photonEvent.CustomData;
                    _serializableEffects.FreezeCell_Effect(data63);
                    break;
            }
        }
    }
}

/*
 
1* - ������ �� ����� �������
10 - ���������� ��������    
11 - ����� ���
12 - �������� ������� � �������

2* - ������ � �����
20 - ��������� ������� ����
21 - ��������� �������� ����
22 - ��������� ������������ ����
23 - �������������� ����

3* - ������ � ������
30 - ����� ���������

4* - ������ � ������������� ������
40 - ������������ ������

5* - ������ � ����������
50 - ��������� ������
51 - ���������� ������

6* - ������ � ���������
60 - �������� ������������� ������ 
61 - ������� ������
62 - �������� ���������� ����� �������
63 - �������� ������ ���������
64 - ������������ OnDisableEffect
 */