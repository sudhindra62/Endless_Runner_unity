
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages the UI for a single daily mission card on the home screen.
/// </summary>
public class DailyMissionCardController : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Title text for the daily mission, e.g., 'Daily Mission'.")]
    public TextMeshProUGUI missionTitle;
    [Tooltip("The fill image for the progress bar. Image type must be set to 'Filled'.")]
    public Image progressFill;
    [Tooltip("Text displaying the mission reward or status.")]
    public TextMeshProUGUI rewardText;

    private void OnEnable()
    {
        DailyMissionManager.OnMissionsUpdated += UpdateUI;
        UpdateUI();
    }

    private void OnDisable()
    {
        DailyMissionManager.OnMissionsUpdated -= UpdateUI;
    }

    private void UpdateUI()
    {
        if (DailyMissionManager.Instance == null) return;

        var missions = DailyMissionManager.Instance.GetActiveMissions();
        if (missions.Count > 0)
        {
            // For now, just display the first mission
            var mission = missions[0];
            missionTitle.text = mission.Data.Description;
            progressFill.fillAmount = (float)mission.CurrentProgress / mission.Data.TargetValue;
            rewardText.text = mission.IsClaimed ? "Claimed" : mission.Data.RewardAmount.ToString();
        }
    }
}
