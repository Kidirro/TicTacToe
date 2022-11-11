using UnityEngine;
using UnityEngine.UI;

public class HistoryCell : MonoBehaviour
{
    [HideInInspector]
    public PlayerInfo Player;

    [HideInInspector]
    public CardInfo Card;

    [SerializeField]
    private Image _cardImage;

    public void SetCard(PlayerInfo player, CardInfo info)
    {
        Player = player;
        Card = info;

        _cardImage.sprite =  (player.SideId == 1) ? info.CardImageP1 : info.CardImageP2;
    }

    public void OnPointerDown()
    {
        Managers.HistoryManager.Instance.StartTap(Card,Player);
    }

    public void OnPointerUp()
    {
        Managers.HistoryManager.Instance.EndTap();
    }

}
