using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : Singleton<CardManager>
{

    [SerializeField] private bool _isNeedGizmos;

    [SerializeField]
    private Card _cardPrefab;

    private List<Card> _cardList = new List<Card>();

    public List<Card> CardAvaible
    {
        get { return _cardList; }
    }

    private static List<CardInfo> _cardInfoListStatic = new List<CardInfo>();

    public static int CardListCount()
    {
        if (_cardInfoListStatic == null) _cardInfoListStatic = new List<CardInfo>();
        return _cardInfoListStatic.Count;
    }

    public static void CardListAdd(CardInfo card)
    {
        if (_cardInfoListStatic == null) _cardInfoListStatic = new List<CardInfo>();
        if (_cardInfoListStatic.IndexOf(card) == -1)
        {
            _cardInfoListStatic.Add(card);
        }

    }

    public static void CardListRemove(CardInfo card)
    {
        if (_cardInfoListStatic == null) _cardInfoListStatic = new List<CardInfo>();
        if (_cardInfoListStatic.IndexOf(card) != -1)
        {
            _cardInfoListStatic.Remove(card);
        }

    }

    public static void CardListClear()
    {
        _cardInfoListStatic = new List<CardInfo>();
    }

    public List<Card> CreateDeck()
    {
        List<Card> newDeck = new List<Card>();
        foreach (CardInfo cardInfo in _cardInfoListStatic)
        {
            for (int i = 0; i < cardInfo.CardCount; i++)
            {
                Card card = GameObject.Instantiate(_cardPrefab);
                //card.name = card.Info.CardName;
                Debug.Log(card);
                card.SetCardInfo(cardInfo);
                card.Info.CardBonusManacost = 0;
                card.gameObject.SetActive(false);
                card.SetTransformParent(transform);
                newDeck.Add(card);
            }
        }
        return newDeck;
    }
}
