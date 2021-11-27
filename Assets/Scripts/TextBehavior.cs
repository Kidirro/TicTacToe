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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            LanguageController.ChangeLanguage((Language)Random.Range(0,2));
        }
    }
}

public enum TextType
{
    NewGame,
    EndTurn
}
