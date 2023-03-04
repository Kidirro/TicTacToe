using Cards;
using History;
using History.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class HistoryCell : MonoBehaviour
{
    private PlayerInfo _player;

    public CardInfo Card;

    private Image _cardImage;

    #region Interfaces

    private IHistoryViewService _historyViewService;

    #endregion
    
    [Inject]
    private void Construct(IHistoryViewService historyViewService)
    {
        _historyViewService = historyViewService;
    }
    
    public void SetCard(PlayerInfo player, CardInfo info)
    {
        _player = player;
        Card = info;

        _cardImage.sprite = (player.SideId == 1) ? info.CardImageP1 : info.CardImageP2;
    }

    public void OnPointerDown()
    {
        _historyViewService.StartTap(Card, _player);
    }

    public void OnPointerUp()
    {
        _historyViewService.EndTap();
    }
}