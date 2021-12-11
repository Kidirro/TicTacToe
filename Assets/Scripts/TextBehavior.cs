using UnityEngine;
using UnityEngine.UI;

public class TextBehavior : MonoBehaviour
{
    [HideInInspector]
    public Text TextObj;
    public TextType Type;

    private void Awake()
    {
        TextObj = this.GetComponent<Text>();
        LanguageController.AddText(this);
        LanguageController.ChangeLanguage(Language.English);
    }
}

public enum TextType
{
    NewGame,
    EndTurn
}
