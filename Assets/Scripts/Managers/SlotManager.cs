using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : Singleton<SlotManager>
{
    #region Fields
    /// <summary>
    /// Нужна ли прорисовка гизмоса
    /// </summary>
    [SerializeField, Tooltip("Нужна ли прорисовка гизмоса")]
    private bool _isNeedGizmos = true;

    /// <summary>
    /// Разрешение экрана по умолчанию
    /// </summary>
    [SerializeField, Tooltip("Разрешение экрана по умолчанию")]
    private Vector2 _defaultScreenResolution;


    /// <summary>
    /// Отступ от нижней границы
    /// </summary>
    [SerializeField, Tooltip("Отступ от нижней границы")]
    private float _buttonBorder;

    /// <summary>
    /// Отступ от границ экрана по бокам
    /// </summary>
    [SerializeField,Tooltip("Отступ от границ экрана по бокам")]
    private float _widthBorder;

    /// <summary>
    /// Ширина одной карты
    /// </summary>
    [SerializeField, Tooltip("Ширина одной карты")]
    private float _widthCard;


    /// <summary>
    /// Позиция колоды
    /// </summary>
    [SerializeField, Tooltip("Позиция колоды (По X)")]
    private float _deckPosition;

    /// <summary>
    /// Дельта угла карт
    /// </summary>
    [SerializeField, Tooltip("Дельта угла карт")]
    private float _angleDelta;

    /// <summary>
    /// Дельта высоты карт
    /// </summary>
    [SerializeField,Tooltip("Дельта высоты карт")]
    private float _heightDelta;

    /// <summary>
    /// Количество слотов
    /// </summary>
    [SerializeField,Tooltip("Максимальное количество слотов")]
    private int _slotsCount;

    /// <summary>
    /// Высота пересдачи карты
    /// </summary>
    [SerializeField,Tooltip("Высота пересдачи карты")]
    private int _rechangerHeight;

    /// <summary>
    /// Пересдаватель
    /// </summary>
    [SerializeField, Tooltip("Пересдаватель")]
    private Rechanger _rechanger;


    /// <summary>
    /// Использован ли заменитель на этом ходу
    /// </summary>
    private bool _isRechangerUsed = false;



    #endregion 

    private void OnDrawGizmos()
    {
        if (_isNeedGizmos)
        {
            float PositionY = Camera.main.pixelHeight * (_buttonBorder) / _defaultScreenResolution.y;
            float StepX = (Camera.main.pixelWidth - _widthBorder*2 + _widthCard*_slotsCount) / (_slotsCount + 1);
            Gizmos.color = Color.green;
            for (int i = 0; i < _slotsCount; i++)
            {
                float posY = PositionY + (Mathf.Sin(Mathf.PI * (i + 1) / (_slotsCount + 1))) * _heightDelta;
                Gizmos.DrawCube(Camera.main.ScreenToWorldPoint(new Vector2(_widthBorder - _widthCard * _slotsCount/2 + StepX * (i + 1), posY)), Vector3.one / 2);
            }
            Gizmos.DrawLine(Camera.main.ScreenToWorldPoint(new Vector2(_widthBorder - _widthCard * _slotsCount / 2, PositionY)),
                Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth - _widthBorder + _widthCard * _slotsCount / 2, PositionY)));
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
        player.HandPool[card].SetTransformPosition(_deckPosition, _buttonBorder);
        player.HandPool[card].SetTransformRotation(0);

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
        player.DeckPool[player.DeckPool.Count - 1].SetTransformRotation(0);
        player.DeckPool[player.DeckPool.Count - 1].SetTransformParent(CardManager.Instance.transform);
    }

    public void RemoveCard(PlayerInfo player, Card card)
    {
        if (player.HandPool.Count == 0) return;
        if (!player.HandPool.Contains(card)) return;

        player.DeckPool.Add(card);
        player.HandPool.Remove(card);
        player.DeckPool[player.DeckPool.Count - 1].gameObject.SetActive(false);
        player.DeckPool[player.DeckPool.Count - 1].SetTransformRotation(0);
        player.DeckPool[player.DeckPool.Count - 1].SetTransformParent(CardManager.Instance.transform);
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
            _rechanger.Show();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            RemoveCard(PlayerManager.Instance.GetCurrentPlayer(), 0);
            _rechanger.Hide();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UpdateCardPosition(false);
        }
    }

    public void ResetRechanher()
    {
        _isRechangerUsed = false;
        Debug.Log(_isRechangerUsed);
    }

    public void UpdateCardPosition( bool instantly = true, Card card = null)
    {
        float currentCount = PlayerManager.Instance.GetCurrentPlayer().HandPool.Count;
        int curIndex = PlayerManager.Instance.GetCurrentPlayer().HandPool.IndexOf(card);
        if (curIndex != -1) { currentCount -= 1;
            PlayerManager.Instance.GetCurrentPlayer().HandPool.Remove(card);

            PlayerManager.Instance.GetCurrentPlayer().HandPool.Add(card);
        }
        Debug.Log(PlayerManager.Instance.GetCurrentPlayer().HandPool.IndexOf(card));

        

            float PositionY = Camera.main.pixelHeight * (_buttonBorder) / _defaultScreenResolution.y;
            float StepPos = (_defaultScreenResolution.x - _widthBorder * 2 + _widthCard * currentCount) / (currentCount + 1);
            float StepRot = (_angleDelta * 2) / (currentCount + 1);

            for (int i = 0; i < currentCount; i++)
            {
                float posY = PositionY + (Mathf.Sin(Mathf.PI * (i + 1) / (currentCount + 1))) * _heightDelta * Camera.main.pixelHeight / _defaultScreenResolution.y;
                Vector2 finPosition = new Vector2((_widthBorder + StepPos * (i + 1) - _widthCard * currentCount / 2) * Camera.main.pixelWidth / _defaultScreenResolution.x, posY);
                PlayerManager.Instance.GetCurrentPlayer().HandPool[i].HandPosition = finPosition;
                PlayerManager.Instance.GetCurrentPlayer().HandPool[i].SetTransformPosition(finPosition.x, finPosition.y, instantly);

                PlayerManager.Instance.GetCurrentPlayer().HandPool[i].SetTransformSize(0.9f, false);

                PlayerManager.Instance.GetCurrentPlayer().HandPool[i].HandRotation = _angleDelta - StepRot * (i + 1);
                PlayerManager.Instance.GetCurrentPlayer().HandPool[i].SetTransformRotation(_angleDelta - StepRot * (i + 1), instantly);

            }
    }

    public void ShowRechanger()
    {
        Debug.Log(_isRechangerUsed);

        if (!_isRechangerUsed)
        {
            _rechanger.Show();
        }
    }


    public void HideRechanger()
    {

        _rechanger.Hide();
    }

    public void UseRechanger(Card card) 
    { 
        if (!_isRechangerUsed)
        {
            _isRechangerUsed = true;
            RemoveCard(PlayerManager.Instance.GetCurrentPlayer(),card);
            AddCard(PlayerManager.Instance.GetCurrentPlayer());
            UpdateCardPosition(false);
        }
    }

    public bool IsOnRechanger(float posY)
    {
        if (_isRechangerUsed) return false;
        return posY < _rechanger.Height;
    }
}
