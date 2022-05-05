using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New card", menuName = "Card")]
public class CardInfo : ScriptableObject
{
    public Image CardImage;
    public string CardName;
    public string CardDescription;

    [Range(0,5)]
    public int CardManacost;
    public UnityEvent ÑardAction;



    public void Test()
    {
        Field.Instance.AddLineUp();
    }

    public void Test2()
    {
    }
    public void Test3()
    {
        Field.Instance.SwapVerticalLines(0, 1);
    }
}
