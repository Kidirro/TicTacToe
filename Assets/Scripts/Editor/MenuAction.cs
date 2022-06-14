using UnityEditor;
using UnityEngine;

public class MenuAction 
{
    [MenuItem("TTTP Actions/Cards/Add Card in hand")]
    public static void AddHand()
    {
        SlotManager.Instance.AddCard(PlayerManager.Instance.GetCurrentPlayer());
        SlotManager.Instance.UpdateCardPosition(false);
    }

    [MenuItem("TTTP Actions/Cards/Remove Card from hand")]
    public static void RemoveHand()
    {
        SlotManager.Instance.RemoveCard(PlayerManager.Instance.GetCurrentPlayer(),0);
        SlotManager.Instance.UpdateCardPosition(false);
    }

    [MenuItem("TTTP Actions/Cards/Update Card position")]
    public static void UpdateHand()
    {
        SlotManager.Instance.UpdateCardPosition(false);
    }

    [MenuItem("TTTP Actions/Mana/Reset Mana")]
    public static void ResetMana()
    {
        ManaManager.Instance.ResetMana();
        ManaManager.Instance.UpdateManaUI();
    }

    [MenuItem("TTTP Actions/Mana/Add 1 bonus Mana")]
    public static void Bonus1Mana()
    {
        ManaManager.Instance.AddBonusMana(1);
        ManaManager.Instance.UpdateManaUI();
    }

}
