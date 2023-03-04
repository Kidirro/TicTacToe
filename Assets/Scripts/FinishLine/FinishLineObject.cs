using System.Collections;
using System.Collections.Generic;
using GameState;
using Managers;
using Players;
using UnityEngine;

namespace FinishLine
{
    public class FinishLineObject : Line
    {
        private static float _finishCountFrame = 25;

        public static float AnimationTime
        {

            get => _finishCountFrame*2;
        }


        public IEnumerator FinishLineCleaning(List<Vector2Int> ids, int score)
        {
            int current_player = PlayerManager.Instance.GetCurrentPlayer().SideId;
            Vector2Int id1 = new Vector2Int(ids[0].x, ids[0].y);
            Vector2Int id2 = new Vector2Int(ids[ids.Count - 1].x, ids[ids.Count - 1].y);

            SetAlphaFinishLine(0);
            //SetWidthScreenCord(Field.Instance.CellList[0][0].CellSize * Field.Instance.LineWidthPercent);
            SetPositions(Field.Instance.CellList[id1.x][id1.y].Position, Field.Instance.CellList[id2.x][id2.y].Position);
            float j = 0;
            while (j < _finishCountFrame)
            {
                j++;
                SetAlphaFinishLine(j / _finishCountFrame);
                yield return null;
            }

            GameplayManager.Instance.AddScore(score, current_player);

            foreach (Vector2Int id in ids)
            {
                Field.Instance.CellList[id.x][id.y].SetFigure(CellFigure.none,isQueue:false);
            }

            while (j > 0)
            {
                j--;
                SetAlphaFinishLine(j / _finishCountFrame);
                yield return null;
            }
            Field.Instance.AddToFinishLineList(this);
        }
    }
}
