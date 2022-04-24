using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{

    [SerializeField] private bool _isNeedGizmos;
    [SerializeField] private Vector2 _defaultScreenResolution;
    [SerializeField] private Vector2 _screenBorderX;
    [SerializeField] private Vector2 _screenBorderY;

    private List<GameObject> _cardList = new List<GameObject>();

    [SerializeField] private GameObject _testCard;

    private void OnDrawGizmos()
    {
        if (_isNeedGizmos)
        {
            float StartPositionX = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth * ((_screenBorderX.x) / _defaultScreenResolution.x), 0)).x;
            float EndPositionX = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth * ((_defaultScreenResolution.x - _screenBorderX.y) / _defaultScreenResolution.x), 0)).x;

            float StartPositionY = Camera.main.ScreenToWorldPoint(new Vector2(0, Camera.main.pixelHeight * (_screenBorderY.x) / _defaultScreenResolution.y)).y;
            float EndPositionY = Camera.main.ScreenToWorldPoint(new Vector2(0, Camera.main.pixelHeight * (_defaultScreenResolution.y - _screenBorderY.y) / _defaultScreenResolution.y)).y;
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(new Vector2(StartPositionX, StartPositionY), new Vector2(EndPositionX, StartPositionY));
            Gizmos.DrawLine(new Vector2(EndPositionX, StartPositionY), new Vector2(EndPositionX, EndPositionY));
            Gizmos.DrawLine(new Vector2(EndPositionX, EndPositionY), new Vector2(StartPositionX, EndPositionY));
            Gizmos.DrawLine(new Vector2(StartPositionX, EndPositionY), new Vector2(StartPositionX, StartPositionY));

        }
    }

    private void AddCard()
    {
        _cardList.Add(Instantiate(_testCard));
        float StartPositionX = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth * ((_screenBorderX.x) / _defaultScreenResolution.x), 0)).x;
        float EndPositionX = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth * ((_defaultScreenResolution.x - _screenBorderX.y) / _defaultScreenResolution.x), 0)).x;
        float StartPositionY = Camera.main.ScreenToWorldPoint(new Vector2(0, Camera.main.pixelHeight * (_screenBorderY.x) / _defaultScreenResolution.y)).y;
        float EndPositionY = Camera.main.ScreenToWorldPoint(new Vector2(0, Camera.main.pixelHeight * (_defaultScreenResolution.y - _screenBorderY.y) / _defaultScreenResolution.y)).y;
        float y_pos = (EndPositionY - StartPositionY) / 2;
        float x_pos = (EndPositionX - StartPositionX) / (_cardList.Count + 1);

        for (int i = 0; i < _cardList.Count; i++)
        {
            _cardList[i].transform.position = new Vector2(StartPositionX + x_pos * (i + 1), StartPositionY + y_pos);
            _cardList[i].GetComponent<Card>().HandPosition = new Vector2(StartPositionX + x_pos * (i + 1), StartPositionY + y_pos);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) AddCard();
    }
}
