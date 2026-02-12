using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Manages the state and logic for daily missions.
/// </summary>
public partial class DailyMissionManager : MonoBehaviour
{
    public static DailyMissionManager Instance { get; private set; }

    // =======================
    // EVENTS
    // =======================

    public static event Action OnMissionProgress;
    public static event Action OnMissionsRefreshed;
    public static event Action OnMissionsUpdated;

    // =======================
    // DATA
    // =======================

    [Header("Mission Data")]
    [SerializeField] private List<MissionData> missionPool = new List<MissionData>();

    // 🔴 CHANGED: private → public (REQUIRED)
    public List<MissionStatus> activeMissions = new List<MissionStatus>();

    private const int NumDailyMissions = 3;

    private const string LastMissionDateKey = "LastMissionDate";
    private const string ActiveMissionsKey = "ActiveMissions";

    // =======================
    // LIFECYCLE
    // =======================

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeMissions();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // =======================
    // INITIALIZATION
    // =======================

    private void InitializeMissions()
    {
        string today = DateTime.UtcNow.ToString("yyyy-MM-dd");
        string lastDate = PlayerPrefs.GetString(LastMissionDateKey, "");

        if (lastDate != today)
        {
            AssignNewMissions();
            PlayerPrefs.SetString(LastMissionDateKey, today);
            SaveMissions();
            OnMissionsRefreshed?.Invoke();
        }
        else
        {
            LoadMissions();
        }

        OnMissionsUpdated?.Invoke();
    }

    private void AssignNewMissions()
    {
        activeMissions.Clear();
        List<MissionData> availableMissions = new List<MissionData>(missionPool);

        for (int i = 0; i < NumDailyMissions && availableMissions.Count > 0; i++)
        {
            int index = UnityEngine.Random.Range(0, availableMissions.Count);
            activeMissions.Add(new MissionStatus(availableMissions[index]));
            availableMissions.RemoveAt(index);
        }
    }

    // =======================
    // PUBLIC API
    // =======================

    public void UpdateMissionProgress(MissionType type, int amount)
    {
        bool updated = false;

        foreach (var mission in activeMissions)
        {
            if (mission.Data.type == type && !mission.IsClaimed)
            {
                mission.CurrentProgress += amount;
                updated = true;
            }
        }

        if (updated)
        {
            SaveMissions();
            OnMissionProgress?.Invoke();
            OnMissionsUpdated?.Invoke();
        }
    }

    // 🔹 ORIGINAL (KEPT)
    public void ClaimMissionReward(int missionIndex)
    {
        ClaimReward(missionIndex);
    }

    // 🔹 ADDED OVERLOAD (UI USES missionId)
    public void ClaimMissionReward(string missionId)
    {
        for (int i = 0; i < activeMissions.Count; i++)
        {
            if (activeMissions[i].Data.missionId == missionId)
            {
                ClaimReward(i);
                return;
            }
        }
    }

    private void ClaimReward(int missionIndex)
    {
        if (missionIndex < 0 || missionIndex >= activeMissions.Count) return;

        var mission = activeMissions[missionIndex];
        if (mission.IsClaimed || mission.CurrentProgress < mission.Data.goal) return;

        mission.IsClaimed = true;

        if (CurrencyManager.Instance != null)
        {
            if (mission.Data.rewardType == MissionRewardType.Coins)
                CurrencyManager.Instance.AddCoins(mission.Data.rewardAmount);
            else
                CurrencyManager.Instance.AddGems(mission.Data.rewardAmount);
        }

        SaveMissions();
        OnMissionsUpdated?.Invoke();
    }

    public IReadOnlyList<MissionStatus> GetActiveMissions()
    {
        return activeMissions;
    }

    // =======================
    // SAVE / LOAD
    // =======================

    [Serializable]
    private class MissionStatusList
    {
        public List<MissionStatus> list = new List<MissionStatus>();
    }

    private void SaveMissions()
    {
        PlayerPrefs.SetString(
            ActiveMissionsKey,
            JsonUtility.ToJson(new MissionStatusList { list = activeMissions })
        );
        PlayerPrefs.Save();
    }

    private void LoadMissions()
    {
        string json = PlayerPrefs.GetString(ActiveMissionsKey, "");

        if (!string.IsNullOrEmpty(json))
        {
            MissionStatusList wrapper = JsonUtility.FromJson<MissionStatusList>(json);
            activeMissions = wrapper.list ?? new List<MissionStatus>();
        }
        else
        {
            AssignNewMissions();
            OnMissionsRefreshed?.Invoke();
        }
    }
}
