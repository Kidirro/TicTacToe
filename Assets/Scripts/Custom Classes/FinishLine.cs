using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : Line
{
    private float _finishLineSpeed = 4f;

    public IEnumerator FinishLineCleaning(List<Vector2Int> ids, int score)
    {
        int current_player = PlayerManager.Instance.GetCurrentPlayer().SideId;
        Vector2Int id1 = new Vector2Int(ids[0].x, ids[0].y);
        Vector2Int id2 = new Vector2Int(ids[ids.Count - 1].x, ids[ids.Count - 1].y);

        SetAlphaFinishLine(0);
        //SetWidthScreenCord(Field.Instance.CellList[0][0].CellSize * Field.Instance.LineWidthPercent);
        SetPositions(Field.Instance.CellList[id1.x][id1.y].Position, Field.Instance.CellList[id2.x][id2.y].Position);
        float j = 0;
        while (j < 100f / _finishLineSpeed)
        {
            j++;
            SetAlphaFinishLine(j / (100f / _finishLineSpeed));
            yield return null;
        }
        UIController.AddScore(current_player, score);
        foreach (Vector2Int id in ids)
        {
            Field.Instance.CellList[id.x][id.y].SetState(CellState.empty);
        }

        while (j > 0)
        {
            j--;
            SetAlphaFinishLine(j / (100f / _finishLineSpeed));
            yield return null;
        }
        Field.Instance.AddToFinishLineList(this);
    }
}
