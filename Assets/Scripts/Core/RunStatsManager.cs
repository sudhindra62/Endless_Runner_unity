using UnityEngine;

public class RunStatsManager : MonoBehaviour
{
    public static RunStatsManager Instance { get; private set; }

    public int CoinsCollectedThisRun { get; private set; }
    public float DistanceTraveledThisRun { get; private set; }

    private Vector3 lastPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (PlayerController.Instance != null)
        {
            lastPosition = PlayerController.Instance.transform.position;
        }
    }

    private void Update()
    {
        if (PlayerController.Instance != null && !PlayerController.Instance.IsDead)
        {
            float distance = Vector3.Distance(PlayerController.Instance.transform.position, lastPosition);
            DistanceTraveledThisRun += distance;
            lastPosition = PlayerController.Instance.transform.position;
        }
    }

    public void AddCoin()
    {
        CoinsCollectedThisRun++;
    }

    public void ResetStats()
    {
        CoinsCollectedThisRun = 0;
        DistanceTraveledThisRun = 0;
        if (PlayerController.Instance != null)
        {
            lastPosition = PlayerController.Instance.transform.position;
        }
    }
}
