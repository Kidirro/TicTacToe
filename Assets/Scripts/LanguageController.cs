using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class LanguageController : MonoBehaviour
{
    private static TextAsset _languageFile = Resources.Load<TextAsset>("Language");
    private static List<TextBehavior> _listText = new List<TextBehavior>();

    public static void AddText(TextBehavior text)
    {
        _listText.Add(text);
    }

    public static void ChangeLanguage(Language l)
    {
        foreach (TextBehavior ta in _listText)
        {
            string patern = "(?<=" + l.ToString() + ":" + ta.Type.ToString() + ":).*";
            ta.TextObj.text=Regex.Match(_languageFile.text, patern).ToString();
        }
    }

    public static void DeleteText(TextBehavior text)
    {
        _listText.Remove(text);
    }
}

public enum Language
{
    Russian,
    English
}