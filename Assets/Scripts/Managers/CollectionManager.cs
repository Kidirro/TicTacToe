using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectionManager : MonoBehaviour
{
    [SerializeField]
    private List<CardCollection> _cardCollections = new List<CardCollection>();

    [Header("Texts"), SerializeField]
    private TextMeshProUGUI _cardUnlockValue;
    
    [SerializeField]
    private TextMeshProUGUI _moneyValue;


    private void Start()
    {
        CreateCardPull();
        _cardUnlockValue.text = CoinManager.CoinPerUnlock.ToString();
    }

    private void OnEnable()
    {
        UpdateCardState();
        UpdateTexts();
    }

    private void UpdateCardState()
    {
        foreach (CardCollection card in _cardCollections) card.UpdateUI();
    }

    private void UpdateTexts()
    {
        _moneyValue.text = CoinManager.AllCoins.ToString();
    }

    private void CreateCardPull()
    {
        CardManager.CardListClear();
        foreach (CardCollection card in _cardCollections)
        {
            if (card.IsUnlock) CardManager.CardListAdd(card.Info);
        }
    }

    public void UnlockRandomCard()
    {
        if (CoinManager.AllCoins < CoinManager.CoinPerUnlock) return;
        List<CardCollection> _lockedList = new List<CardCollection>();
        foreach (CardCollection card in _cardCollections) if (!card.IsUnlock) _lockedList.Add(card);
        if (_lockedList.Count == 0) return;
        int valuerand = Random.Range(0, _lockedList.Count);
        _lockedList[valuerand].UnlockCard();
        _lockedList[valuerand].UpdateUI();
        CoinManager.AllCoins -= CoinManager.CoinPerUnlock;

        _moneyValue.text = CoinManager.AllCoins.ToString();
    }
}
