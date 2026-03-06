
using System.Collections.Generic;
using UnityEngine;

public class MissionUI : MonoBehaviour
{
    [SerializeField] private GameObject missionPanel;
    [SerializeField] private MissionSlotUI[] missionSlots;

    private void Start()
    {
        MissionManager.OnMissionProgress += UpdateMissionProgress;
        MissionManager.OnMissionCompleted += HandleMissionCompletion;
        InitializeMissionUI();
    }

    private void OnDestroy()
    {
        MissionManager.OnMissionProgress -= UpdateMissionProgress;
        MissionManager.OnMissionCompleted -= HandleMissionCompletion;
    }

    private void InitializeMissionUI()
    {
        List<MissionData> activeMissions = MissionManager.Instance.GetActiveMissions();
        for (int i = 0; i < missionSlots.Length; i++)
        {
            if (i < activeMissions.Count)
            {
                missionSlots[i].gameObject.SetActive(true);
                missionSlots[i].SetMission(activeMissions[i]);
            }
            else
            {
                missionSlots[i].gameObject.SetActive(false);
            }
        }
    }

    private void UpdateMissionProgress(MissionData mission)
    {
        foreach (var slot in missionSlots)
        {
            // This is not efficient, but it's simple for now.
            // A better approach would be to have a dictionary mapping mission to slot.
            if (slot.gameObject.activeSelf)
            {
                // Refresh all slots, or find the correct one to update.
                InitializeMissionUI(); 
            }
        }
    }

    private void HandleMissionCompletion(MissionData mission)
    {
        // Potentially show a completion animation or message
        InitializeMissionUI();
    }
}
