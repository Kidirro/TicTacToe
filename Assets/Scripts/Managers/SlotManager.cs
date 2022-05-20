using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : Singleton<SlotManager>
{
    #region Fields
    /// <summary>
    /// Нужна ли прорисовка гизмоса
    /// </summary>
    [SerializeField]
    private bool _isNeedGizmos = true;

    /// <summary>
    /// Разрешение экрана по умолчанию
    /// </summary>
    [SerializeField]
    private Vector2 _defaultScreenResolution;


    /// <summary>
    /// Отступ от нижней границы
    /// </summary>
    [SerializeField]
    private float _buttonBorder;


    /// <summary>
    /// Позиция колоды
    /// </summary>
    [SerializeField]
    private float _deckPosition;



    /// <summary>
    /// Количество слотов
    /// </summary>
    [SerializeField]
    private int _slotsCount;

    /// <summary>
    /// Позиция слотов
    /// </summary>
    private List<Vector2> _slots = new List<Vector2>();

    /// <summary>
    /// Позиция колоды
    /// </summary>
    private Vector2 _deck;

    #endregion 

    private void OnDrawGizmos()
    {
        if (_isNeedGizmos)
        {
            //float StartPositionX = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth * ((_screenBorderX.x) / _defaultScreenResolution.x), 0)).x;
            //float EndPositionX = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth * ((_defaultScreenResolution.x - _screenBorderX.y) / _defaultScreenResolution.x), 0)).x;

            float PositionY = Camera.main.pixelHeight * (_buttonBorder) / _defaultScreenResolution.y;
            float StepX = Camera.main.pixelWidth / (_slotsCount * 2);
            Gizmos.color = Color.green;
            for (int i = 0; i < _slotsCount; i++)
            {
                Gizmos.DrawCube(Camera.main.ScreenToWorldPoint(new Vector2(StepX * (i * 2 + 1), PositionY)), Vector3.one / 2);
            }
            Gizmos.DrawLine(Camera.main.ScreenToWorldPoint(new Vector2(0, _deckPosition)),
                Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth, _deckPosition)));


        }
    }



    public void Initialization()
    {
        float step = Camera.main.pixelWidth / (_slotsCount * 2);
        for (int i = 0; i < _slotsCount; i++)
        {
            _slots.Add(new Vector2(step * (i * 2 + 1), _buttonBorder));

        }
    }


    public void AddCard(PlayerInfo player)
    {
        if (player.HandPool.Count >= _slotsCount) return;
        if (player.DeckPool.Count == 0) return;

        int card = Random.Range(0, player.DeckPool.Count);
        player.HandPool.Add(player.DeckPool[card]);
        player.DeckPool.RemoveRange(card, 1);
        card = player.HandPool.Count - 1;
        player.HandPool[card].SetTransformParent(transform);
        player.HandPool[card].SetTransformPosition(_slots[card].x, _deckPosition);
        player.HandPool[card].HandPosition = _slots[card];
        player.HandPool[card].gameObject.SetActive(true);
        PrintCArd();

    }
    public void RemoveCard(PlayerInfo player, int id)
    {
        if (player.HandPool.Count == 0) return;
        if (id >= player.HandPool.Count) return;

        player.DeckPool.Add(player.HandPool[id]);
        player.HandPool.RemoveRange(id, 1);
        player.DeckPool[player.DeckPool.Count - 1].gameObject.SetActive(false);
        player.DeckPool[player.DeckPool.Count - 1].SetTransformPosition(_slots[0].x, _deckPosition);
        PrintCArd();
    }

    public void RemoveCard(PlayerInfo player, Card card)
    {
        if (player.HandPool.Count == 0) return;
        if (!player.HandPool.Contains(card)) return;

        player.DeckPool.Add(card);
        player.HandPool.Remove(card);
        player.DeckPool[player.DeckPool.Count - 1].gameObject.SetActive(false);
        player.DeckPool[player.DeckPool.Count - 1].SetTransformPosition(_slots[0].x, _deckPosition);
        PrintCArd();
    }

    private void PrintCArd()
    {
        for (int i = 0; i < PlayerManager.Instance.GetCurrentPlayer().HandPool.Count; i++)
        {
            Debug.LogFormat("[{0}/{1}] карта в руке с названием {2} ", i + 1, PlayerManager.Instance.GetCurrentPlayer().HandPool.Count, PlayerManager.Instance.GetCurrentPlayer().HandPool[i].Info.CardName);
        }
        for (int i = 0; i < PlayerManager.Instance.GetCurrentPlayer().DeckPool.Count; i++)
        {
            Debug.LogFormat("[{0}/{1}] карта в колоде с названием {2} ", i + 1, PlayerManager.Instance.GetCurrentPlayer().DeckPool.Count, PlayerManager.Instance.GetCurrentPlayer().DeckPool[i].Info.CardName);
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AddCard(PlayerManager.Instance.GetCurrentPlayer());
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            RemoveCard(PlayerManager.Instance.GetCurrentPlayer(), 0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UpdateCardPosition(false);
        }
    }


    public void UpdateCardPosition(bool instantly = true)
    {
        for (int i = 0; i < PlayerManager.Instance.GetCurrentPlayer().HandPool.Count; i++)
        {
            Debug.Log(i);
            Debug.Log(_slots[i]);
            PlayerManager.Instance.GetCurrentPlayer().HandPool[i].HandPosition = _slots[i];
            PlayerManager.Instance.GetCurrentPlayer().HandPool[i].SetTransformPosition(_slots[i].x, _slots[i].y, instantly);
            Debug.Log(PlayerManager.Instance.GetCurrentPlayer().HandPool[i].Position);
        }
    }
}
