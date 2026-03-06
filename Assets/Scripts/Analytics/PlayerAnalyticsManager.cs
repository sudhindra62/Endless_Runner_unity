using UnityEngine;

public class PlayerAnalyticsManager : MonoBehaviour
{
    public static PlayerAnalyticsManager Instance { get; private set; }

    private SessionAnalyticsData currentSession;

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

    private void OnEnable()
    {
        PlayerController.OnPlayerAction += TrackDodge;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerAction -= TrackDodge;
    }

    public void StartNewSession()
    {
        currentSession = new SessionAnalyticsData();
        currentSession.StartSession();
    }

    public void EndSession()
    {
        if (currentSession != null)
        {
            currentSession.EndSession();
            // In a real-world scenario, you would send this data to a server.
            Debug.Log("Session Ended: " + JsonUtility.ToJson(currentSession));
        }
    }

    public void TrackPlayerDeath(string cause, float distance)
    {
        if (currentSession != null)
        {
            currentSession.RecordDeath(cause, distance);
        }
    }

    public void TrackDodge(bool success)
    {
        if (currentSession != null)
        {
            currentSession.RecordDodge(success);
        }
    }

    public void TrackCombo(int peak)
    {
        if (currentSession != null)
        {
            currentSession.RecordCombo(peak);
        }
    }

    public void TrackRevive()
    {
        if (currentSession != null)
        {
            currentSession.RecordRevive();
        }
    }

    public void TrackBossEncounter(string bossName, bool survived)
    {
        if (currentSession != null)
        {
            currentSession.RecordBossEncounter(bossName, survived);
        }
    }
}
