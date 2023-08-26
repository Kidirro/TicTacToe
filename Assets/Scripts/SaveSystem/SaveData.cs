using System;
using System.Collections.Generic;
using Cards.CustomType;
using Map;

[Serializable]
public class MapData : ISaveble
{
    public List<Road> Roads = new();
    public List<Node> Nodes = new();
}

[Serializable]
public class CardBoolData : ISaveble
{
    public Dictionary<int,bool> BoolData = new();
}

[Serializable]
public class CardListData : ISaveble
{
    public List<int> List = new();
}

public interface ISaveble
{
}