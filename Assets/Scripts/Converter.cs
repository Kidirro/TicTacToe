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
}
