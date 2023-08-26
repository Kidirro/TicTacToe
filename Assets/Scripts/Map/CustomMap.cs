using System; 
using System.Collections;
using System.Collections.Generic;
using Map;
using UnityEngine;


[CreateAssetMenu(fileName = "New map", menuName = "Map"),]
public class CustomMap : ScriptableObject
{
    [SerializeField]
    public ListWrapper<List<Node>> nodePositionsIterations = new();

    public int id;


    public List<List<Node>> GetNodePostition()
    {
        List<List<Node>> result = new List<List<Node>>();



        return result;
    }

    [Serializable]
    public class ListWrapper <T>
    {
        public List<T> List;
        
        public static implicit operator ListWrapper<T>(List<T> list)
        {
            return new ListWrapper<T>() {List = list};
        }  
        
        public static implicit  operator List<T>(ListWrapper<T> wrapper)
        {
            return new List<T>(wrapper.List);
        }
    }
}
