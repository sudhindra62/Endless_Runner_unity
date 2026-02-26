using UnityEngine;
using System;

public class GameFlowController : MonoBehaviour
{
    public static GameFlowController Instance { get; private set; }

    public static event Action OnPauseForDeath;

    private TimeControlManager timeControlManager;
    private bool runActive = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        timeControlManager = GetComponent<TimeControlManager>();
    }

    public void StartRun()
    {
        runActive = true;
        timeControlManager.ResumeTime();

        ReviveManager.Instance?.ResetForNewRun();

        Debug.Log("Run started");
    }

    public void PauseForDeath()
    {
        if (!runActive) return;

        runActive = false;
        timeControlManager.PauseTime();

        Debug.Log("Run paused (player died)");
        OnPauseForDeath?.Invoke();
    }

    public void ResumeAfterRevive()
    {
        runActive = true;
        timeControlManager.ResumeTime();

        Debug.Log("Run resumed after revive");
    }

    public void EndRunFinal()
    {
        runActive = false;
        timeControlManager.ResumeTime();

        Debug.Log("Run ended permanently (no revive)");
        if (EndOfRunManager.Instance != null)
        {
            EndOfRunManager.Instance.ShowRunSummary();
        }
    }

    public bool IsRunActive() => runActive;
}
