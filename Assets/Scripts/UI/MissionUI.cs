using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// Manages the UI for displaying the list of active missions.
/// Listens to events from the MissionManager to keep the display synchronized.
/// 
/// --- Inspector Setup ---
/// 1. Attach this script to a UI panel for missions.
/// 2. Create three identical UI prefabs/groups for displaying a single mission.
/// 3. Assign the parent Transform of each mission UI group to the 'missionUIParents' list.
/// 4. Ensure each mission group has a consistent hierarchy with Text and Image components for the script to find.
/// </summary>
public class MissionUI : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("A list of parent GameObjects, one for each of the 3 active mission displays.")]
    [SerializeField] private List<GameObject> missionUIGroups;

    #region Unity Lifecycle & Event Subscription

    private void OnEnable()
    {
        MissionManager.OnMissionsUpdated += HandleMissionsUpdated;
        // Initial update
        if(MissionManager.Instance != null) UpdateMissionDisplay(MissionManager.Instance.GetActiveMissions());
    }

    private void OnDisable()
    {
        MissionManager.OnMissionsUpdated -= HandleMissionsUpdated;
    }

    #endregion

    private void HandleMissionsUpdated()
    {
        if (MissionManager.Instance != null)
        {
            UpdateMissionDisplay(MissionManager.Instance.GetActiveMissions());
        }
    }

    private void UpdateMissionDisplay(List<ProjectMissionData> activeMissions)
    {
        if (activeMissions == null) return;

        for (int i = 0; i < missionUIGroups.Count; i++)
        {
            var group = missionUIGroups[i];
            if (group == null) continue;

            if (i < activeMissions.Count)
            {
                group.SetActive(true);
                PopulateMissionUI(group, new ActiveMissionState { Definition = activeMissions[i] });
            }
            else
            {
                group.SetActive(false);
            }
        }
    }

    private void PopulateMissionUI(GameObject group, ActiveMissionState missionState)
    {
        if (missionState == null || missionState.Definition == null) return;

        var mission = missionState.Definition;

        // Find UI components within the group - safely
        TMP_Text descriptionText = group.transform.Find("DescriptionText")?.GetComponent<TMP_Text>();
        TMP_Text rewardText = group.transform.Find("RewardText")?.GetComponent<TMP_Text>();
        Image progressBar = group.transform.Find("ProgressBar")?.GetComponent<Image>();

        if (descriptionText != null)
        {
            descriptionText.text = GetMissionDescription(mission);
        }

        if (rewardText != null)
        { 
            rewardText.text = $"{mission.rewardAmount} {mission.rewardType}";
        }

        if (progressBar != null)
        {
            progressBar.fillAmount = missionState.GetProgress01();
        }
    }

    private string GetMissionDescription(ProjectMissionData mission)
    {
        switch (mission.type)
        {
            case MissionType.RunDistance: return $"Run {mission.goal}m in one go";
            case MissionType.CollectCoins: return $"Collect {mission.goal} coins in one run";
            case MissionType.SurviveTime: return $"Survive for {mission.goal} seconds";
            default: return "Unknown Mission";
        }
    }
}
