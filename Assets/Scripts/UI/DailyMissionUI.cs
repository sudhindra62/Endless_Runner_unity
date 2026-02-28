
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

    public void Setup(MissionState mission)
    {
        this.currentMission = mission;
        UpdateMissionDisplay(mission);
    }

    private void UpdateMissionDisplay(MissionState mission)
    {
        if (mission.data.missionId != currentMission.data.missionId) return;

        descriptionText.text = mission.data.Description;

        float progressRatio = (float)mission.progress / mission.data.TargetValue;
        progressBar.value = progressRatio;
        progressText.text = $"{mission.progress} / {mission.data.TargetValue}";

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
