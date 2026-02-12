using UnityEngine;
using System;
using System.Collections.Generic;

public partial class MilestoneManager : MonoBehaviour
{
    public static MilestoneManager Instance { get; private set; }

    private List<MilestoneData> allMilestones = new List<MilestoneData>();
    private Dictionary<string, long> milestoneProgress = new Dictionary<string, long>();
    private HashSet<string> claimedMilestones = new HashSet<string>();

    public static event Action<string, long, long> OnProgressUpdated;
    public static event Action<MilestoneData> OnMilestoneClaimed;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadMilestonesFromResources();
            LoadProgress();
        }
    }

    private void LoadMilestonesFromResources()
    {
        allMilestones.AddRange(Resources.LoadAll<MilestoneData>("Milestones"));
    }

    public void IncrementProgress(string milestoneID, int amount)
    {
        if (!milestoneProgress.ContainsKey(milestoneID) || IsClaimed(milestoneID))
            return;

        long currentProgress = milestoneProgress[milestoneID];
        long goal = GetMilestoneByID(milestoneID).goal;

        if (currentProgress >= goal) return;

        milestoneProgress[milestoneID] += amount;
        OnProgressUpdated?.Invoke(milestoneID, milestoneProgress[milestoneID], goal);
        SaveProgress();
    }

    public void ClaimMilestone(string milestoneID)
    {
        MilestoneData milestone = GetMilestoneByID(milestoneID);
        if (milestone == null || IsClaimed(milestoneID)) return;

        if (GetProgress(milestoneID) >= milestone.goal)
        {
            claimedMilestones.Add(milestoneID);
            OnMilestoneClaimed?.Invoke(milestone);
            SaveProgress();
        }
    }

    private void SaveProgress()
    {
        foreach (var milestone in allMilestones)
        {
            PlayerPrefs.SetString(
                $"Milestone_Progress_{milestone.milestoneID}",
                milestoneProgress[milestone.milestoneID].ToString()
            );

            PlayerPrefs.SetInt(
                $"Milestone_Claimed_{milestone.milestoneID}",
                claimedMilestones.Contains(milestone.milestoneID) ? 1 : 0
            );
        }

        PlayerPrefs.Save();
    }

    private void LoadProgress()
    {
        foreach (var milestone in allMilestones)
        {
            string key = $"Milestone_Progress_{milestone.milestoneID}";
            long progress = PlayerPrefs.HasKey(key)
                ? long.Parse(PlayerPrefs.GetString(key))
                : 0;

            milestoneProgress[milestone.milestoneID] = progress;

            if (PlayerPrefs.GetInt($"Milestone_Claimed_{milestone.milestoneID}", 0) == 1)
                claimedMilestones.Add(milestone.milestoneID);
        }
    }

    public List<MilestoneData> GetAllMilestones() => allMilestones;
    public long GetProgress(string id) => milestoneProgress.ContainsKey(id) ? milestoneProgress[id] : 0;
    public bool IsClaimed(string id) => claimedMilestones.Contains(id);
    public MilestoneData GetMilestoneByID(string id) =>
        allMilestones.Find(m => m.milestoneID == id);
}
