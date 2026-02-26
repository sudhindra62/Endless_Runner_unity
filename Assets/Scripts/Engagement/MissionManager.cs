
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance { get; private set; }

    public static event Action OnMissionsUpdated;

    [Header("Mission Configuration")]
    [SerializeField] private List<MissionData> missionPool = new List<MissionData>();

    [Header("Active Missions")]
    public List<MissionStatus> activeMissions = new List<MissionStatus>();

    private const int NumDailyMissions = 3;
    private const string LastMissionDateKey = "LastMissionDate";
    private const string ActiveMissionsKey = "ActiveMissions";

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

    private void InitializeMissions()
    {
        string today = DateTime.UtcNow.ToString("yyyy-MM-dd");
        string lastDate = PlayerPrefs.GetString(LastMissionDateKey, "");

        if (lastDate != today)
        {
            AssignNewDailyMissions();
            PlayerPrefs.SetString(LastMissionDateKey, today);
        }
        
        LoadMissions();

        if (activeMissions.Count == 0)
        {
            AssignInitialPersistentMissions();
        }

        OnMissionsUpdated?.Invoke();
    }

    private void AssignNewDailyMissions()
    {
        activeMissions.RemoveAll(m => m.Data.category == MissionCategory.Daily);

        List<MissionData> dailyMissionPool = missionPool.Where(m => m.category == MissionCategory.Daily).ToList();
        List<MissionData> availableMissions = new List<MissionData>(dailyMissionPool);

        for (int i = 0; i < NumDailyMissions && availableMissions.Count > 0; i++)
        {
            int index = UnityEngine.Random.Range(0, availableMissions.Count);
            activeMissions.Add(new MissionStatus(availableMissions[index]));
            availableMissions.RemoveAt(index);
        }
        
        SaveMissions();
    }

    private void AssignInitialPersistentMissions()
    {
        List<MissionData> persistentMissionPool = missionPool.Where(m => m.category == MissionCategory.Persistent).ToList();
        activeMissions.AddRange(persistentMissionPool.Take(3).Select(m => new MissionStatus(m)));
        SaveMissions();
    }

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
            OnMissionsUpdated?.Invoke();
        }
    }

    public void ClaimMissionReward(string missionId)
    {
        MissionStatus mission = activeMissions.FirstOrDefault(m => m.Data.missionId == missionId);
        if (mission == null || mission.IsClaimed || mission.CurrentProgress < mission.Data.goal) return;

        mission.IsClaimed = true;

        if (CurrencyManager.Instance != null)
        {
            if (mission.Data.rewardType == MissionRewardType.Coins)
                CurrencyManager.Instance.AddCoins(mission.Data.rewardAmount);
            else
                CurrencyManager.Instance.AddGems(mission.Data.rewardAmount);
        }

        if (mission.Data.category == MissionCategory.Persistent)
        {
            ReplacePersistentMission(missionId);
        }

        SaveMissions();
        OnMissionsUpdated?.Invoke();
    }

    private void ReplacePersistentMission(string completedMissionId)
    {
        activeMissions.RemoveAll(m => m.Data.missionId == completedMissionId);

        MissionData newMission = missionPool.FirstOrDefault(m => 
            m.category == MissionCategory.Persistent && 
            !activeMissions.Any(am => am.Data.missionId == m.missionId));

        if (newMission != null)
        {
            activeMissions.Add(new MissionStatus(newMission));
        }
    }

    public IReadOnlyList<MissionStatus> GetActiveMissions()
    {
        return activeMissions;
    }

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
    }
}
