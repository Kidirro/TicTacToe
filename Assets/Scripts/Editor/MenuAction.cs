using Cards;
using Mana;
using UnityEditor;
using Managers;
using Players;

public class MenuAction 
{
    [MenuItem("TTTP Actions/Cards/Add Card in hand")]
    public static void AddHand()
    {
        CardPoolController.Instance.AddCard(PlayerManager.Instance.GetCurrentPlayer());
        CardPoolController.Instance.UpdateCardPosition(false);
    } 
    
    [MenuItem("TTTP Actions/Cards/Add Card in full hand")]
    public static void AddFullHand()
    {
        CardPoolController.Instance.ChangeCurrentPlayerView(PlayerManager.Instance.GetCurrentPlayer());
        CardPoolController.Instance.UpdateCardPosition(false);
    }

    [MenuItem("TTTP Actions/Cards/Remove Card from hand")]
    public static void RemoveHand()
    {
        CardPoolController.Instance.RemoveCard(PlayerManager.Instance.GetCurrentPlayer(),0);
        CardPoolController.Instance.UpdateCardPosition(false);
    }
    
    [MenuItem("TTTP Actions/Cards/Remove Card from full hand")]
    public static void RemoveFullHand()
    {
        CardPoolController.Instance.ResetHandPool(PlayerManager.Instance.GetCurrentPlayer());
        CardPoolController.Instance.UpdateCardPosition(false);
    }

    [MenuItem("TTTP Actions/Cards/Update Card position")]
    public static void UpdateHand()
    {
        CardPoolController.Instance.UpdateCardPosition(false);
    }

    [MenuItem("TTTP Actions/Mana/Reset Mana")]
    public static void ResetMana()
    {
        ManaManager.Instance.RestoreAllMana();
        ManaManager.Instance.UpdateManaUI();
    }

    [MenuItem("TTTP Actions/Mana/Add 1 bonus Mana")]
    public static void Bonus1Mana()
    {
        ManaManager.Instance.AddBonusMana(1);
        ManaManager.Instance.UpdateManaUI();
    }

    [MenuItem("Language/English")]
    static void English()
    {

        I2.Loc.LocalizationManager.CurrentLanguage = "English";
    }
    [MenuItem("Language/Russian")]
    static void Russian()
    {

        I2.Loc.LocalizationManager.CurrentLanguage = "Russian";
    }
}
