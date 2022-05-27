using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : Singleton<SlotManager>
{
    #region Fields
    /// <summary>
    /// ����� �� ���������� �������
    /// </summary>
    [SerializeField, Tooltip("����� �� ���������� �������")]
    private bool _isNeedGizmos = true;

    /// <summary>
    /// ���������� ������ �� ���������
    /// </summary>
    [SerializeField, Tooltip("���������� ������ �� ���������")]
    private Vector2 _defaultScreenResolution;


    /// <summary>
    /// ������ �� ������ �������
    /// </summary>
    [SerializeField, Tooltip("������ �� ������ �������")]
    private float _buttonBorder;

    /// <summary>
    /// ������ �� ������ ������ �� �����
    /// </summary>
    [SerializeField,Tooltip("������ �� ������ ������ �� �����")]
    private float _widthBorder;

    /// <summary>
    /// ������ ����� �����
    /// </summary>
    [SerializeField, Tooltip("������ ����� �����")]
    private float _widthCard;


    /// <summary>
    /// ������� ������
    /// </summary>
    [SerializeField, Tooltip("������� ������ (�� X)")]
    private float _deckPosition;

    /// <summary>
    /// ������ ���� ����
    /// </summary>
    [SerializeField, Tooltip("������ ���� ����")]
    private float _angleDelta;

    /// <summary>
    /// ������ ������ ����
    /// </summary>
    [SerializeField,Tooltip("������ ������ ����")]
    private float _heightDelta;

    /// <summary>
    /// ���������� ������
    /// </summary>
    [SerializeField,Tooltip("������������ ���������� ������")]
    private int _slotsCount;

    /// <summary>
    /// ������ ��������� �����
    /// </summary>
    [SerializeField,Tooltip("������ ��������� �����")]
    private int _rechangerHeight;

    /// <summary>
    /// �������������
    /// </summary>
    [SerializeField, Tooltip("�������������")]
    private Rechanger _rechanger;



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
            Debug.LogFormat("[{0}/{1}] ����� � ���� � ��������� {2} ", i + 1, PlayerManager.Instance.GetCurrentPlayer().HandPool.Count, PlayerManager.Instance.GetCurrentPlayer().HandPool[i].Info.CardName);
        }
        for (int i = 0; i < PlayerManager.Instance.GetCurrentPlayer().DeckPool.Count; i++)
        {
            Debug.LogFormat("[{0}/{1}] ����� � ������ � ��������� {2} ", i + 1, PlayerManager.Instance.GetCurrentPlayer().DeckPool.Count, PlayerManager.Instance.GetCurrentPlayer().DeckPool[i].Info.CardName);
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


    public void UpdateCardPosition(bool instantly = true)
    {
        float currentCount = PlayerManager.Instance.GetCurrentPlayer().HandPool.Count;

        float PositionY = Camera.main.pixelHeight * (_buttonBorder) / _defaultScreenResolution.y;
        float StepPos = (Camera.main.pixelWidth - _widthBorder*2 + _widthCard*currentCount) / (currentCount + 1) * Camera.main.pixelWidth / _defaultScreenResolution.x;
        float StepRot = (_angleDelta * 2) / (currentCount + 1);

        for (int i = 0; i < currentCount; i++)
        {
            float posY = PositionY + (Mathf.Sin(Mathf.PI * (i + 1) / (currentCount + 1))) * _heightDelta * Camera.main.pixelHeight / _defaultScreenResolution.y;
            Vector2 finPosition = new Vector2(_widthBorder + StepPos * (i + 1) - _widthCard * currentCount/2, posY);
            PlayerManager.Instance.GetCurrentPlayer().HandPool[i].HandPosition = finPosition;
            PlayerManager.Instance.GetCurrentPlayer().HandPool[i].SetTransformPosition(finPosition.x, finPosition.y, instantly);

            PlayerManager.Instance.GetCurrentPlayer().HandPool[i].SetTransformSize(0.9f,false);

            PlayerManager.Instance.GetCurrentPlayer().HandPool[i].HandRotation = _angleDelta - StepRot * (i + 1);
            PlayerManager.Instance.GetCurrentPlayer().HandPool[i].SetTransformRotation(_angleDelta - StepRot * (i + 1), instantly);

        }
    }
}
