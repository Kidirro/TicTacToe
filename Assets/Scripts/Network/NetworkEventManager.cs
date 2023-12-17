using System;
using CardCollection;
using CardCollection.Interfaces;
using Cards;
using Cards.CustomType;
using Coroutine.Interfaces;
using Effects.Interfaces;
using Emotes.Interfaces;
using ExitGames.Client.Photon;
using Field.Interfaces;
using FinishLine.Interfaces;
using GameState.Interfaces;
using History.Interfaces;
using Mana.Interfaces;
using Network.Interfaces;
using Photon.Pun;
using Photon.Realtime;
using Players.Interfaces;
using UnityEngine;
using Zenject;

namespace Network
{
    public class NetworkEventManager : MonoBehaviour, IOnEventCallback, ICheckEventNetworkService,
        IManaEventNetworkService,
        ICardEventNetworkService, IFigureEventNetworkService, IFreezeEventNetworkService, IEffectEventNetworkService,
        INetworkEventService, IEmotesEventNetworkService
    {
        #region Dependecy

        private ISerializableEffects _serializableEffects;
        private IEffectService _effectService;
        private IPlayerService _playerService;
        private IHistoryService _historyService;
        private IFinishLineService _finishLineService;
        private ICoroutineAwaitService _coroutineService;
        private IManaService _manaService;
        private IManaUIService _manaUIService;
        private IFieldFigureService _fieldFigureService;
        private IEmoteService _emoteService;
        private ICollectionData _collectionData;

        [Inject]
        private void Construct(
            ISerializableEffects serializableEffects,
            IEffectService effectService,
            IPlayerService playerService,
            IHistoryService historyService,
            IFinishLineService finishLineService,
            ICoroutineAwaitService coroutineService,
            IManaService manaService,
            IManaUIService manaUIService,
            IFieldFigureService fieldFigureService,
            IEmoteService emoteService,
            ICollectionData collectionData)
        {
            _serializableEffects = serializableEffects;
            _effectService = effectService;
            _playerService = playerService;
            _historyService = historyService;
            _finishLineService = finishLineService;
            _coroutineService = coroutineService;
            _manaService = manaService;
            _manaUIService = manaUIService;
            _fieldFigureService = fieldFigureService;
            _emoteService = emoteService;
            _collectionData = collectionData;
        }

        #endregion

        private bool _isOnline;
        private Action _newTurnAction;

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

        public void RaiseEventMasterChecker()
        {
            if (!_isOnline) return;
            RaiseEventOptions ro = new RaiseEventOptions {Receivers = ReceiverGroup.Others};
            SendOptions so = new SendOptions {Reliability = true};
            PhotonNetwork.RaiseEvent(10, null, ro, so);
        }

        public void RaiseEventEndTurn()
        {
            if (!_isOnline) return;

            RaiseEventOptions ro = new RaiseEventOptions {Receivers = ReceiverGroup.Others};
            SendOptions so = new SendOptions {Reliability = true};
            PhotonNetwork.RaiseEvent(11, null, ro, so);
        }

        public void RaiseEventAwaitTime(float time)
        {
            if (!_isOnline) return;

            RaiseEventOptions ro = new RaiseEventOptions {Receivers = ReceiverGroup.Others};
            SendOptions so = new SendOptions {Reliability = true};
            PhotonNetwork.RaiseEvent(12, time, ro, so);
        }

        #endregion

        #region Mana_Event

        public void RaiseEventIncreaseMana(int value, bool isOverMax = false)
        {
            if (!_isOnline) return;
            RaiseEventOptions ro = new RaiseEventOptions {Receivers = ReceiverGroup.Others};
            SendOptions so = new SendOptions {Reliability = true};
            PhotonNetwork.RaiseEvent(20, new Vector2Int(value, isOverMax ? 1 : 0), ro, so);
        }

        public void RaiseEventAddBonusMana(int value)
        {
            if (!_isOnline) return;
            RaiseEventOptions ro = new RaiseEventOptions {Receivers = ReceiverGroup.Others};
            SendOptions so = new SendOptions {Reliability = true};
            PhotonNetwork.RaiseEvent(21, value, ro, so);
        }

        public void RaiseEventIncreaseMaxMana(int value)
        {
            if (!_isOnline) return;
            RaiseEventOptions ro = new RaiseEventOptions {Receivers = ReceiverGroup.Others};
            SendOptions so = new SendOptions {Reliability = true};
            PhotonNetwork.RaiseEvent(22, value, ro, so);
        }


        /// <summary>
        /// ��� -1 AllMana
        /// </summary>
        /// <param name="value"></param>
        public void RaiseEventRestoreMana(int value)
        {
            if (!_isOnline) return;
            RaiseEventOptions ro = new RaiseEventOptions {Receivers = ReceiverGroup.Others};
            SendOptions so = new SendOptions {Reliability = true};
            PhotonNetwork.RaiseEvent(23, value, ro, so);
        }

        #endregion

        #region Card_Event

        public void RaiseEventCardInvoke(CardInfo card)
        {
            if (!_isOnline) return;
            RaiseEventOptions ro = new RaiseEventOptions {Receivers = ReceiverGroup.Others};
            SendOptions so = new SendOptions {Reliability = true};
            PhotonNetwork.RaiseEvent(30, card.CardId, ro, so);
        }

        #endregion

        #region Figure_Event

        public void RaiseEventPlaceInCell(Vector2Int id)
        {
            if (!_isOnline) return;
            RaiseEventOptions ro = new RaiseEventOptions {Receivers = ReceiverGroup.Others};
            SendOptions so = new SendOptions {Reliability = true};
            PhotonNetwork.RaiseEvent(40, id, ro, so);
        }

        #endregion

        #region Freeze_event

        public void RaiseEventFreezeCell(Vector2Int id)
        {
            if (!_isOnline) return;

            RaiseEventOptions ro = new RaiseEventOptions {Receivers = ReceiverGroup.Others};
            SendOptions so = new SendOptions {Reliability = true};
            PhotonNetwork.RaiseEvent(50, id, ro, so);
        }

        public void RaiseEventUnFreezeCell(Vector2Int id)
        {
            if (!_isOnline) return;

            RaiseEventOptions ro = new RaiseEventOptions {Receivers = ReceiverGroup.Others};
            SendOptions so = new SendOptions {Reliability = true};
            PhotonNetwork.RaiseEvent(51, id, ro, so);
        }

        #endregion

        #region Effect_event

        public void RaiseEventAddEffect(Action action)
        {
            if (!_isOnline) return;
            if (_serializableEffects.GetIdSerializableAction(action) == -1) return;
            Debug.Log($"Send effect! {_serializableEffects.GetIdSerializableAction(action)}");
            RaiseEventOptions ro = new RaiseEventOptions {Receivers = ReceiverGroup.Others};
            SendOptions so = new SendOptions {Reliability = true};
            PhotonNetwork.RaiseEvent(60, _serializableEffects.GetIdSerializableAction(action), ro, so);
        }

        public void RaiseEventAddEffect(int action)
        {
            if (!_isOnline) return;
            RaiseEventOptions ro = new RaiseEventOptions {Receivers = ReceiverGroup.Others};
            SendOptions so = new SendOptions {Reliability = true};
            PhotonNetwork.RaiseEvent(60, action, ro, so);
        }

        public void RaiseEventClearEffect(int action)
        {
            if (!_isOnline) return;
            RaiseEventOptions ro = new RaiseEventOptions {Receivers = ReceiverGroup.Others};
            SendOptions so = new SendOptions {Reliability = true};
            PhotonNetwork.RaiseEvent(61, action, ro, so);
        }

        public void RaiseEventUpdateEffect(int action, int value)
        {
            if (!_isOnline) return;
            Vector2Int data = new Vector2Int(action, value);
            RaiseEventOptions ro = new RaiseEventOptions {Receivers = ReceiverGroup.Others};
            SendOptions so = new SendOptions {Reliability = true};
            PhotonNetwork.RaiseEvent(62, data, ro, so);
        }

        public void RaiseEventAddFreezeEffect(Vector2Int position)
        {
            if (!_isOnline) return;
            RaiseEventOptions ro = new RaiseEventOptions {Receivers = ReceiverGroup.Others};
            SendOptions so = new SendOptions {Reliability = true};
            PhotonNetwork.RaiseEvent(63, position, ro, so);
        }

        #endregion

        #region Emote_Events

        public void RaiseEventShowEmote(int id)
        {
            if (!_isOnline) return;

            RaiseEventOptions ro = new RaiseEventOptions {Receivers = ReceiverGroup.Others};
            SendOptions so = new SendOptions {Reliability = true};
            PhotonNetwork.RaiseEvent(70, id, ro, so);
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
                    _finishLineService.MasterChecker(_playerService.GetCurrentPlayer().SideId,
                        isNeedEvent: false);
                    break;
                case 11:
                    _newTurnAction?.Invoke();
                    break;
                case 12:
                    float data12 = (float) photonEvent.CustomData;
                    _coroutineService.AddAwaitTime(data12);
                    break;
                case 20:
                    Vector2Int data20 = (Vector2Int) photonEvent.CustomData;
                    _manaService.IncreaseMana(data20.x, data20.y == 1);
                    _manaUIService.UpdateManaUI();
                    break;
                case 21:
                    _manaService.AddBonusMana((int) photonEvent.CustomData);
                    _manaService.RestoreAllMana();
                    _manaUIService.UpdateManaUI();
                    break;
                case 22:
                    _manaService.IncreaseMaxMana((int) photonEvent.CustomData);
                    _manaUIService.UpdateManaUI();
                    break;
                case 23:
                    int data23 = (int) photonEvent.CustomData;
                    if (data23 == -1) _manaService.RestoreAllMana();
                    else _manaService.RestoreMana(data23);
                    _manaUIService.UpdateManaUI();
                    break;
                case 30:
                    CardInfo data30 = _collectionData.GetCardFromId((int) photonEvent.CustomData);
                    _historyService.AddHistoryCard(_playerService.GetCurrentPlayer(), data30);
                    break;
                case 40:
                    Vector2Int data40 = (Vector2Int) photonEvent.CustomData;
                    _fieldFigureService.PlaceInCell(data40, false);
                    break;
                case 50:
                    Vector2Int data50 = (Vector2Int) photonEvent.CustomData;
                    _fieldFigureService.FreezeCell(data50, false);
                    break;
                case 51:
                    Vector2Int data51 = (Vector2Int) photonEvent.CustomData;
                    _fieldFigureService.ResetSubStateWithPlaceFigure(data51, false);
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
                case 70:
                    int data70 = (int) photonEvent.CustomData;
                    _emoteService.ShowEmote(data70);
                    break;
            }
        }

        public void SetIsOnline(bool state)
        {
            _isOnline = state;
        }

        public void SetNewTurnAction(Action action)
        {
            _newTurnAction = action;
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