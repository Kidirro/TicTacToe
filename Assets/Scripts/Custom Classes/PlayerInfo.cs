using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo
{
    public PlayerType EntityType;

    public int SideId;

}


public enum PlayerType
{
    Human, 
    AI
}