using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Analytic;
using Analytic.Interfaces;
using Zenject;

public class SearchingEnemyWindow : MonoBehaviour
{
    [Header("Timer properties"), SerializeField]
    private TextMeshProUGUI _timerText;

    public static float TimePass;

    #region Dependency

    private IMatchEventsAnalyticService _matchEventsAnalyticService;
    
    [Inject]
    private void Construct(IMatchEventsAnalyticService matchEventsAnalyticService)
    {
        _matchEventsAnalyticService = matchEventsAnalyticService;
    }

    #endregion
 
    
    public void StartLoadingProcess()
    {
        _matchEventsAnalyticService.Player_Try_Find_Match();
        StartCoroutine(ILoadingProcess());
    }

    public void StopLoadingProcess()
    {
        StopAllCoroutines();
        _matchEventsAnalyticService.Player_Cancel_Find_Match(TimePass);
    }

    public IEnumerator ILoadingProcess()
    {
        DateTime beginTime = DateTime.Now;
        while (true)
        {
            TimeSpan timeSpan = DateTime.Now - beginTime;
            _timerText.text = timeSpan.ToString(@"mm\:ss");
            TimePass = (float) timeSpan.TotalSeconds;

            yield return null;
        }
    }
}
