using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{

    void Start()
    {
        DontDestroyOnLoad(this);
        Application.targetFrameRate = 60;
    }

}
