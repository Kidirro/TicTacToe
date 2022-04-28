using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New card", menuName = "Card")]
public class CardInfo : ScriptableObject
{
    public int CardId;
    public Image CardImage;
    public string CardName;
    public string CardDescription;
    public UnityEvent ÑardAction;



    public void Test()
    {
        Debug.Log("++");
    }

    public void Test2()
    {
        Field.Instance.SwapHorizontalLines(0, 1);
    }
    public void Test3()
    {
        Field.Instance.SwapVerticalLines(0, 1);
    }
}
