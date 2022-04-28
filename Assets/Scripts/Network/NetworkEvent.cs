using ExitGames.Client.Photon;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkEvent : MonoBehaviour, IOnEventCallback
{

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
        
        PhotonPeer.RegisterType(typeof(Vector2Int), 228, Converter.SerializeVector2Int, Converter.DeserializeVector2Int);

    }



    public static void RaiseEventCellState(Vector2Int id)
    {
        RaiseEventOptions RO = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions SO = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(3, id, RO,SO);
    }

    public static void RaiseEventEndTurn()
    {
        RaiseEventOptions RO = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions SO = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(2, true, RO, SO);
    }


    void IOnEventCallback.OnEvent(EventData photonEvent)
    {
        Debug.Log(photonEvent.Code);
        switch (photonEvent.Code)
        {
            case 2:
                UIController.Instance.NewTurn(false);
                TurnController.NewTurn(true);
                break;
            case 3:
                Vector2Int id = (Vector2Int)photonEvent.CustomData;
                Field.Instance.CellList[id.x][id.y].SetState(PlayerManager.Instance.GetCurrentPlayer().SideId);
                TurnController.MasterChecker(id);
                break;
        }


    }
}
