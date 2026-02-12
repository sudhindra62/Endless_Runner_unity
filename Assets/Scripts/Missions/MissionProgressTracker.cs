using UnityEngine;

/// <summary>
/// Acts as a bridge between gameplay events and the daily mission system.
/// SAFE MODE: Logic unchanged, only references fixed.
/// </summary>
public class MissionProgressTracker : MonoBehaviour
{
    public static MissionProgressTracker Instance { get; private set; }

    // 🔹 FIX: Correct manager reference
    private DailyMissionManager missionManager;

    private float distanceAccumulator;
    private float timeAccumulator;

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
        // 🔹 FIX: Correct singleton source
        missionManager = DailyMissionManager.Instance;
    }

    private void Update()
    {
        UpdateTime(Time.deltaTime);
    }

    // =======================
    // PUBLIC GAMEPLAY HOOKS
    // =======================

    public void OnRunStarted()
    {
        distanceAccumulator = 0f;
        timeAccumulator = 0f;
    }

    public void OnCoinCollected(int amount = 1)
    {
        if (missionManager != null)
        {
            missionManager.UpdateMissionProgress(MissionType.CollectCoins, amount);
        }
    }

    public void OnJump()
    {
        if (missionManager != null)
        {
            missionManager.UpdateMissionProgress(MissionType.JumpTotal, 1);
        }
    }

    // =======================
    // INTERNAL PROGRESS
    // =======================

    private void UpdateTime(float deltaTime)
    {
        timeAccumulator += deltaTime;

        if (timeAccumulator >= 1.0f)
        {
            int progress = (int)timeAccumulator;

            if (progress > 0 && missionManager != null)
            {
                missionManager.UpdateMissionProgress(MissionType.SurviveTime, progress);
            }

            timeAccumulator -= progress;
        }
    }

    public void UpdateDistance(float distance)
    {
        distanceAccumulator += distance;

        if (distanceAccumulator >= 1.0f)
        {
            int progress = (int)distanceAccumulator;

            if (progress > 0 && missionManager != null)
            {
                missionManager.UpdateMissionProgress(MissionType.RunDistance, progress);
            }

            distanceAccumulator -= progress;
        }
    }
}
