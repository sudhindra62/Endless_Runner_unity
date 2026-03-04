using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public RunSessionData runSessionData;
    public PlayerAnalyticsManager playerAnalyticsManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        playerAnalyticsManager = FindObjectOfType<PlayerAnalyticsManager>();
        if (playerAnalyticsManager == null)
        {
            GameObject analyticsObject = new GameObject("PlayerAnalyticsManager");
            playerAnalyticsManager = analyticsObject.AddComponent<PlayerAnalyticsManager>();
        }
    }

    private void Start()
    {
        playerAnalyticsManager.StartNewSession();
    }

    private void OnApplicationQuit()
    {
        playerAnalyticsManager.EndSession();
    }
}
