using System.Collections.Generic;
using Cards;
using Cards.CustomType;

public class PlayerInfo
{
    public PlayerType EntityType;

    public int SideId;

    public List<CardModel> HandPool = new List<CardModel>();
    public List<CardModel> DeckPool = new List<CardModel>();
    public List<CardModel> FullDeckPool = new List<CardModel>();
}


public enum PlayerType
{
    Human,
    AI
}