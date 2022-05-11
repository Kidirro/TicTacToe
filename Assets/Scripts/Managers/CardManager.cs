using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : Singleton<CardManager>
{

    [SerializeField] private bool _isNeedGizmos;

    [SerializeField]
    private Card _cardPrefab;

    private List<Card> _cardList = new List<Card>();

    [SerializeField]
    private List<CardInfo> _cardInfoList = new List<CardInfo>();

    private void Awake()
    {
        foreach (CardInfo cardInfo in _cardInfoList)
        {
            Card card = GameObject.Instantiate(_cardPrefab);
            card.SetCardInfo(cardInfo);
            card.gameObject.SetActive(false);
            card.transform.SetParent(transform);
            _cardList.Add(card);
        }
    }
}
