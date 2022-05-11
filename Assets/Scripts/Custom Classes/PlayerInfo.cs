using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo
{
    public PlayerType EntityType;

    public int SideId;

    public List<Card> HandPool = new List<Card>();
    public List<Card> DeckPool = new List<Card>();
    public List<Card> FullDeckPool = new List<Card>();

}


public enum PlayerType
{
    Human, 
    AI
}