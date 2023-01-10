using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Managers;

public class SearchingEnemyWindow : MonoBehaviour
{
    [Header("Timer properties"), SerializeField]
    private TextMeshProUGUI _timerText;

    [Header("Timer properties"), SerializeField]
    private Transform _loadingBar;

    public static float TimePass;
    
    public void StartLoadingProcess()
    {
        AnalitycManager.Player_Try_Find_Match();
        StartCoroutine(ILoadingProcess());
    }

    public void StopLoadingProcess()
    {
        StopAllCoroutines();
    }

    public IEnumerator ILoadingProcess()
    {
        DateTime beginTime = DateTime.Now;
        while (true)
        {
            TimeSpan timeSpan = DateTime.Now - beginTime;
            _timerText.text = timeSpan.ToString(@"mm\:ss");
            TimePass = (float) timeSpan.TotalSeconds;
            _loadingBar.transform.rotation = Quaternion.Euler(0, 0, (_loadingBar.rotation.eulerAngles.z - Mathf.Sin(timeSpan.Seconds) - 2) % 360);

            yield return null;
        }
    }
}
