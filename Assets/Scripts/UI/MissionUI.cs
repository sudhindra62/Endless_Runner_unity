
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Displays active missions and their progress to the player.
/// Subscribes to MissionManager events to stay updated.
/// </summary>
public class MissionUI : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private MissionManager missionManager;

    [Header("UI Prefabs & Containers")]
    [SerializeField] private GameObject missionSlotPrefab;
    [SerializeField] private Transform missionContainer;

    private List<MissionSlotUI> missionSlots = new List<MissionSlotUI>();

    private void OnEnable()
    {
        if (missionManager == null) missionManager = MissionManager.Instance;
        
        MissionManager.OnMissionProgress += UpdateMissionSlot;
        MissionManager.OnMissionCompleted += HandleMissionCompletion; // Maybe play an animation

        GenerateMissionSlots();
    }

    private void OnDisable()
    {
        MissionManager.OnMissionProgress -= UpdateMissionSlot;
        MissionManager.OnMissionCompleted -= HandleMissionCompletion;
    }

    private void GenerateMissionSlots()
    {
        // Clear old slots
        foreach (Transform child in missionContainer)
        {
            Destroy(child.gameObject);
        }
        missionSlots.Clear();

        List<MissionData> activeMissions = missionManager.GetActiveMissions();
        
        foreach (var mission in activeMissions)
        {
            GameObject slotInstance = Instantiate(missionSlotPrefab, missionContainer);
            MissionSlotUI slotUI = slotInstance.GetComponent<MissionSlotUI>();
            slotUI.Setup(mission);
            missionSlots.Add(slotUI);
        }
    }

    private void UpdateMissionSlot(MissionData mission)
    {
        MissionSlotUI slotToUpdate = missionSlots.Find(s => s.GetMissionData() == mission);
        if (slotToUpdate != null)
        {
            slotToUpdate.RefreshProgress();
        }
    }
    
    private void HandleMissionCompletion(MissionData mission)
    {
        // Here you might trigger a UI animation or effect for completion
        // After animation, you could replace the slot.
        GenerateMissionSlots(); // Simple regeneration for now
    }
}
