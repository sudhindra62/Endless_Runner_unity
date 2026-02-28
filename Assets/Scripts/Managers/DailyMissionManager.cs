
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class DailyMissionManager : MonoBehaviour
{
    public static DailyMissionManager Instance { get; private set; }

    public List<DailyMissionData> activeMissions = new List<DailyMissionData>();

    public static event Action OnMissionsUpdated;
    public static event Action<DailyMissionData> OnMissionProgress;

    [Header("Mission Configuration")]
    [SerializeField] private List<ProjectMissionData> allMissions;
    [SerializeField] private int numberOfDailyMissions = 3;

    private const string LastRefreshKey = "LastMissionRefreshTime";
    private const string ActiveMissionsKey = "ActiveMissionsData";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        ServiceLocator.Register<DailyMissionManager>(this);
    }

    private void Start()
    {
        if (ShouldRefreshMissions())
        {
            RefreshDailyMissions();
        }
        else
        {
            LoadMissions();
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) {
            Instance = null;
            ServiceLocator.Unregister<DailyMissionManager>();
        }
    }

    private bool ShouldRefreshMissions()
    {
        string lastRefreshString = PlayerPrefs.GetString(LastRefreshKey, null);
        if (string.IsNullOrEmpty(lastRefreshString))
        {
            return true;
        }

        long lastRefreshBinary;
        if (long.TryParse(lastRefreshString, out lastRefreshBinary))
        {
            DateTime lastRefreshTime = DateTime.FromBinary(lastRefreshBinary);
            return DateTime.UtcNow.Date > lastRefreshTime.Date;
        }

        return true;
    }

    public void RefreshDailyMissions()
    {
        activeMissions.Clear();
        System.Random rng = new System.Random();
        var availableMissions = allMissions.OrderBy(x => rng.Next()).ToList();

        for (int i = 0; i < numberOfDailyMissions && i < availableMissions.Count; i++)
        {
            DailyMissionData newMission = new DailyMissionData
            {
                missionData = availableMissions[i],
                currentProgress = 0,
                isCompleted = false,
                isClaimed = false
            };
            activeMissions.Add(newMission);
        }

        PlayerPrefs.SetString(LastRefreshKey, DateTime.UtcNow.ToBinary().ToString());
        SaveMissions();
        OnMissionsUpdated?.Invoke();
    }

    public void UpdateMissionProgress(string missionId, int amount)
    {
        DailyMissionData mission = activeMissions.Find(m => m.missionData.missionId == missionId);
        if (mission != null && !mission.isCompleted)
        {
            mission.currentProgress += amount;
            OnMissionProgress?.Invoke(mission);

            if (mission.currentProgress >= mission.missionData.missionGoal)
            {
                mission.isCompleted = true;
            }
            SaveMissions();
            OnMissionsUpdated?.Invoke();
        }
    }

    public void ClaimMissionReward(string missionId)
    {
        DailyMissionData mission = activeMissions.Find(m => m.missionData.missionId == missionId);
        if (mission != null && mission.isCompleted && !mission.isClaimed)
        {
            var currencyManager = ServiceLocator.Get<CurrencyManager>();
            if (currencyManager != null)
            {
                currencyManager.AddGems(mission.missionData.rewardAmount);
            }
            mission.isClaimed = true;
            SaveMissions();
            OnMissionsUpdated?.Invoke();
        }
    }

    private void SaveMissions()
    {
        string json = JsonUtility.ToJson(new MissionSaveData { missions = activeMissions });
        PlayerPrefs.SetString(ActiveMissionsKey, json);
    }

    private void LoadMissions()
    {
        string json = PlayerPrefs.GetString(ActiveMissionsKey, null);
        if (!string.IsNullOrEmpty(json))
        {
            MissionSaveData saveData = JsonUtility.FromJson<MissionSaveData>(json);
            activeMissions = saveData.missions;
            OnMissionsUpdated?.Invoke();
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }

    [System.Serializable]
    private class MissionSaveData
    {
        public List<DailyMissionData> missions;
    }
}
