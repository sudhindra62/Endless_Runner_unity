
using UnityEngine;
using System;

public class GameFlowController : Singleton<GameFlowController>
{
    public static event Action OnRunStart;
    public static event Action OnRunEnd;

    public void StartRun()
    {
        Debug.Log("New run started.");
        OnRunStart?.Invoke();
        // Additional logic to start a game run
    }

    public void EndRun()
    {
        Debug.Log("Run ended.");
        OnRunEnd?.Invoke();
        // Additional logic to end a game run
    }
}
