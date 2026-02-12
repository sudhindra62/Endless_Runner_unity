using UnityEngine;

public class GameFlowController : MonoBehaviour
{
    public static GameFlowController Instance { get; private set; }

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
    }

    /* -------------------------
     * RUN LIFECYCLE
     * ------------------------- */

    public void StartRun()
    {
        runActive = true;
        Time.timeScale = 1f;

        ReviveManager.Instance?.ResetForNewRun();

        Debug.Log("Run started");
    }

    public void PauseForDeath()
    {
        runActive = false;
        Time.timeScale = 0f;

        Debug.Log("Run paused (player died)");
    }

    public void ResumeAfterRevive()
    {
        runActive = true;
        Time.timeScale = 1f;

        Debug.Log("Run resumed after revive");
    }

    public void EndRunFinal()
    {
        runActive = false;
        Time.timeScale = 1f;

        Debug.Log("Run ended permanently (no revive)");
        // Phase 8 will hook Game Over UI here
    }

    public bool IsRunActive() => runActive;
}
