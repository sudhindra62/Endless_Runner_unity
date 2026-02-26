using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Controls the UI for a single daily mission slot.
/// </summary>
public class DailyMissionUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private Slider progressBar;
    [SerializeField] private Button claimButton;
    [SerializeField] private GameObject completedOverlay;

    private MissionState currentMission;

    void OnEnable()
    {
        claimButton.onClick.AddListener(OnClaimButtonPressed);
        DailyMissionManager.OnMissionProgress += UpdateMissionDisplay;
    }

    void OnDisable()
    {
        claimButton.onClick.RemoveListener(OnClaimButtonPressed);
        DailyMissionManager.OnMissionProgress -= UpdateMissionDisplay;
    }

    // 🔹 ORIGINAL — KEPT
    public void Setup(MissionState mission)
    {
        this.currentMission = mission;
        UpdateMissionDisplay(mission);
    }

    // 🔹 ADDITIVE OVERLOAD — REQUIRED
    // Allows MissionStatus input without breaking existing systems
    public void Setup(MissionStatus status)
    {
        if (currentMission == null)
            currentMission = new MissionState(status);
        else
            currentMission.SyncFrom(status);

        UpdateMissionDisplay(currentMission);
    }

    // 🔹 ADDITIVE (Action delegate compatibility)
    private void UpdateMissionDisplay()
    {
        if (currentMission != null)
            UpdateMissionDisplay(currentMission);
    }

    // 🔹 ORIGINAL — UNCHANGED
    private void UpdateMissionDisplay(MissionState mission)
    {
        if (mission.data.missionId != currentMission.data.missionId) return;

        descriptionText.text = mission.data.description;

        float progressRatio = (float)mission.progress / mission.data.goal;
        progressBar.value = progressRatio;
        progressText.text = $"{mission.progress} / {mission.data.goal}";

        if (mission.isClaimed)
        {
            claimButton.gameObject.SetActive(false);
            completedOverlay.SetActive(true);
        }
        else
        {
            claimButton.interactable = mission.isComplete;
            claimButton.gameObject.SetActive(true);
            completedOverlay.SetActive(false);
        }
    }

    private void OnClaimButtonPressed()
    {
        DailyMissionManager.Instance.ClaimMissionReward(currentMission.data.missionId);
    }
}
