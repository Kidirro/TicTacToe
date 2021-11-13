using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private List<Image> _manaImageList;

    [SerializeField] private GameObject _RestartMenu;
    static private GameObject _restartMenuStatic;

    private void Awake()
    {
        _restartMenuStatic = _RestartMenu;
    }

    static public void ShowRestartMenu(bool val)
    {
        _restartMenuStatic.SetActive(val);
    }

    public void RestartGameUI()
    {
        _restartMenuStatic.SetActive(false);
        Field.Instance.RestartGame();
        TurnController.Restart();
    }
}
