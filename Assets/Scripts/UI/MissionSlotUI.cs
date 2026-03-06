
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages the UI for a single mission slot, displaying its progress and details.
/// </summary>
public class MissionSlotUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text missionDescriptionText;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private GameObject completedOverlay;

    private MissionData assignedMission;

    /// <summary>
    /// Sets up the UI slot with the data from a specific mission.
    /// </summary>
    public void Setup(MissionData mission)
    {
        assignedMission = mission;
        RefreshProgress();
    }

    /// <summary>
    /// Updates the visual elements of the mission slot to reflect current progress.
    /// </summary>
    public void RefreshProgress()
    {
        if (assignedMission == null) return;

        missionDescriptionText.text = assignedMission.missionDescription;
        
        if (assignedMission.isCompleted)
        {
            progressSlider.value = 1f;
            progressText.text = "Complete!";
            completedOverlay.SetActive(true);
        }
        else
        {
            progressSlider.value = (float)assignedMission.currentProgress / assignedMission.goal;
            progressText.text = $"{assignedMission.currentProgress} / {assignedMission.goal}";
            completedOverlay.SetActive(false);
        }
    }

    public MissionData GetMissionData()
    {
        return assignedMission;
    }
}
