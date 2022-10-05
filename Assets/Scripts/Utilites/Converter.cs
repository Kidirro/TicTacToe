using System;
using UnityEngine;

public class Converter { 


    public static object DeserializeVector2Int(byte[] data)
    {
        Vector2Int result = new Vector2Int();
        result.x = BitConverter.ToInt32(data, 0);
        result.y = BitConverter.ToInt32(data, 4);

        return result;
    }

    public static byte[] SerializeVector2Int(object data)
    {
        Vector2Int obj = (Vector2Int)data;
        byte[] result = new byte[8];

        BitConverter.GetBytes(obj.x).CopyTo(result, 0);
        BitConverter.GetBytes(obj.y).CopyTo(result, 4);

        return result;
    }  
    public static object DeserializeVector3Int(byte[] data)
    {
        Vector3Int result = new Vector3Int();
        result.x = BitConverter.ToInt32(data, 0);
        result.y = BitConverter.ToInt32(data, 4);
        result.z = BitConverter.ToInt32(data, 8);

        return result;
    }

    public static byte[] SerializeVector3Int(object data)
    {
        Vector3Int obj = (Vector3Int)data;
        byte[] result = new byte[8];

        BitConverter.GetBytes(obj.x).CopyTo(result, 0);
        BitConverter.GetBytes(obj.y).CopyTo(result, 4);
        BitConverter.GetBytes(obj.z).CopyTo(result, 8);

        return result;
    }   
    
}
