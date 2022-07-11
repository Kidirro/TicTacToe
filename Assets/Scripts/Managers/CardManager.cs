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
    

    [SerializeField]
    private List<CardInfo> _cardInfoList = new List<CardInfo>();

    public void Initialization()
    {
        foreach (CardInfo cardInfo in _cardInfoList)
        {
            for (int i = 0; i < cardInfo.CardCount; i++)
            {
                Card card = GameObject.Instantiate(_cardPrefab);
                card.SetCardInfo(cardInfo);
                card.gameObject.SetActive(false);
                card.SetTransformParent(transform);
                _cardList.Add(card);
            }
        }
    } 
    public List<Card> CreateDeck()
    {
        List<Card> newDeck = new List<Card>();
        foreach (CardInfo cardInfo in _cardInfoList)
        {
            for (int i = 0; i < cardInfo.CardCount; i++)
            {
                Card card = GameObject.Instantiate(_cardPrefab);
                //card.name = card.Info.CardName;
                Debug.Log(card);
                card.SetCardInfo(cardInfo);
                card.gameObject.SetActive(false); 
                card.SetTransformParent(transform);
                newDeck.Add(card);
            }
        }
        return newDeck;
    }
}
